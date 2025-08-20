/**
 * Este script maneja la lógica para cargar y mostrar citas en una tabla HTML.
 * Incluye funciones para obtener datos de varias APIs locales y renderizar
 * la información de manera eficiente.
 */

document.addEventListener('DOMContentLoaded', () => {
    // Definición de las URLs de la API. Se asume que el servidor local está funcionando.
    const API_URL_CITAS = 'https://localhost:7172/api/Cita';
    const API_URL_SERVICIOS = 'https://localhost:7172/api/Servicio';
    const API_URL_LUGARES = 'https://localhost:7172/api/Lugar';
    const API_URL_USUARIOS = 'https://localhost:7172/api/Usuario';
    const API_URL_CONFIGURACION = 'https://localhost:7172/api/ConfiguracionTurno';

    // Referencias a elementos del DOM
    const citasTableBody = document.getElementById('citas-table');
    const filtroFechaInput = document.getElementById('filtro-fecha');
    
    // Almacenamiento local para los datos de servicios y lugares para evitar llamadas repetidas
    let servicios = [];
    let lugares = [];
    
    /**
     * @async
     * @function fetchRecursos
     * @description Obtiene los datos de servicios y lugares de forma paralela.
     */
    const fetchRecursos = async () => {
        try {
            const [serviciosRes, lugaresRes] = await Promise.all([
                fetch(API_URL_SERVICIOS),
                fetch(API_URL_LUGARES),
            ]);
            servicios = await serviciosRes.json();
            lugares = await lugaresRes.json();
        } catch (error) {
            console.error('Error al cargar servicios o lugares:', error);
        }
    };

    /**
     * @async
     * @function fetchConfiguracionById
     * @description Obtiene una configuración de turno específica por su ID, incluyendo las franjas.
     * @param {string|number} id - El ID de la configuración de turno.
     * @returns {Promise<object|null>} Los datos de la configuración o null si no se encuentra.
     */
    const fetchConfiguracionById = async (id) => {
        try {
            const response = await fetch(`${API_URL_CONFIGURACION}/${id}`);
            if (!response.ok) {
                return null;
            }
            return await response.json();
        } catch (error) {
            console.error(`Error al buscar configuración de turno con ID ${id}:`, error);
            return null;
        }
    };
    
    /**
     * @async
     * @function fetchUsuarioById
     * @description Obtiene los detalles de un usuario por su ID y maneja posibles errores.
     * @param {string|number} id - El ID del usuario.
     * @returns {Promise<object>} Los datos del usuario o un objeto por defecto si falla.
     */
    const fetchUsuarioById = async (id) => {
        // Se utilizan las propiedades 'nombre' y 'correo' para la respuesta JSON.
        const fallbackUser = { nombre: 'Desconocido', correo: '' };
        try {
            const response = await fetch(`${API_URL_USUARIOS}/${id}`);
            if (!response.ok) {
                // Si la respuesta no es exitosa, se lanza un error.
                throw new Error(`Error en la respuesta de la API: ${response.status}`);
            }
            const userData = await response.json();
            
            // Verifica si los datos tienen las propiedades esperadas
            if (userData && (userData.nombre || userData.Value)) {
                console.log(`Datos del usuario ${id} recibidos correctamente:`, userData);
                return userData;
            } else {
                console.warn(`Respuesta inesperada para el usuario con ID ${id}. La respuesta fue:`, userData);
                return fallbackUser;
            }
        } catch (error) {
            // Este bloque se ejecuta si la API no está disponible o si hay un error en la red.
            console.error(`Error al buscar usuario con ID ${id}. Causa: ${error.message}`);
            return fallbackUser;
        }
    };
    
    /**
     * @async
     * @function fetchAllCitas
     * @description Obtiene todas las citas de la API y las renderiza.
     */
    const fetchAllCitas = async () => {
        try {
            // Se asegura de que servicios y lugares estén cargados antes de continuar.
            await fetchRecursos();
            const response = await fetch(API_URL_CITAS);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const citas = await response.json();
            await renderCitasTable(citas);
        } catch (error) {
            console.error('Error al cargar las citas:', error);
            citasTableBody.innerHTML = `<tr><td colspan="6" class="text-center text-danger">Error al cargar las citas. ${error.message}</td></tr>`;
        }
    };

    /**
     * @async
     * @function fetchCitaById
     * @description Obtiene una cita específica por su ID.
     * @param {string|number} id - El ID de la cita.
     * @returns {Promise<object|null>} Los datos de la cita o null si no se encuentra.
     */
    const fetchCitaById = async (id) => {
        try {
            const response = await fetch(`${API_URL_CITAS}/${id}`);
            if (!response.ok) {
                if (response.status === 404) {
                    return null;
                }
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const cita = await response.json();
            return cita;
        } catch (error) {
            console.error(`Error al buscar la cita con ID ${id}:`, error);
            return null;
        }
    };

    /**
     * @async
     * @function renderCitasTable
     * @description Renderiza las citas en la tabla HTML.
     * @param {Array<object>} citas - Un arreglo de objetos de cita.
     */
    const renderCitasTable = async (citas) => {
        citasTableBody.innerHTML = '<tr><td colspan="6" class="text-center text-muted">Cargando citas...</td></tr>';
        
        if (citas.length === 0) {
            citasTableBody.innerHTML = `<tr><td colspan="6" class="text-center text-muted">No hay citas registradas.</td></tr>`;
            return;
        }

        // 1. Obtener todos los IDs de usuario de las citas
        const userIds = citas.map(cita => cita.idUsuario);
        // 2. Crear un conjunto para tener IDs únicos y evitar llamadas duplicadas
        const uniqueUserIds = [...new Set(userIds)];
        
        // 3. Crear un mapa para almacenar los usuarios ya obtenidos y evitar llamadas repetidas
        const usersCache = {};
        
        // 4. Crear un arreglo de promesas para obtener los datos de cada usuario único.
        const userPromises = uniqueUserIds.map(id => fetchUsuarioById(id));
        
        // 5. Esperar a que todas las promesas de usuarios se resuelvan de forma paralela
        const usersData = await Promise.all(userPromises);
        
        // 6. Llenar el caché con los datos de los usuarios
        uniqueUserIds.forEach((id, index) => {
            usersCache[id] = usersData[index];
        });

        // Limpiar el cuerpo de la tabla antes de agregar las filas
        citasTableBody.innerHTML = '';

        // Ahora se puede iterar sobre las citas y usar el caché para obtener los datos de los usuarios.
        for (const cita of citas) {
            const row = document.createElement('tr');
            
            // Obtener nombres de servicios, lugares y la franja horaria
            const servicio = servicios.find(s => s.servicioId === cita.servicioId);
            const lugar = lugares.find(l => l.lugarId === cita.lugarId);
            
            // Obtener el usuario del caché
            const usuario = usersCache[cita.idUsuario];
            
            // Buscar la franja horaria dentro de la configuración de turno específica
            let franjaHorariaTexto = 'N/A';
            const configuracion = await fetchConfiguracionById(cita.turnoId);
            if (configuracion && configuracion.franjas) {
                const franja = configuracion.franjas.find(f => f.franjaId === cita.franjaId);
                if (franja) {
                    franjaHorariaTexto = `${franja.horaInicio.substring(0, 5)} - ${franja.horaFin.substring(0, 5)}`;
                }
            }

            const nombreServicio = servicio ? servicio.nombre : 'Desconocido';
            const nombreLugar = lugar ? lugar.nombre : 'Desconocido';

            // Determinar el estado y la clase de la insignia (badge)
            const estadoNumero = cita.estado;
            let estadoTexto = 'Desconocido';
            let estadoClass = 'bg-secondary';
            
            switch (estadoNumero) {
                case 1:
                    estadoTexto = 'Confirmada';
                    estadoClass = 'bg-success';
                    break;
                case 2:
                    estadoTexto = 'Cancelada';
                    estadoClass = 'bg-danger';
                    break;
                default:
                    estadoTexto = 'Pendiente';
                    estadoClass = 'bg-warning';
                    break;
            }

            // Usar una expresión regular para extraer solo el nombre.
            let nombreUsuario = 'Desconocido';
            if (usuario.nombre) {
                const regex = /{ Value = (.*) }/;
                const match = usuario.nombre.match(regex);
                if (match && match[1]) {
                    nombreUsuario = match[1];
                } else {
                    nombreUsuario = usuario.nombre;
                }
            }

            row.innerHTML = `
                <td>
                    <div class="user-info">
                        <strong>${nombreUsuario}</strong>
                    </div>
                </td>
                <td>${nombreServicio}</td>
                <td>${nombreLugar}</td>
                <td>${new Date(cita.fechaCita).toLocaleDateString()}</td>
                <td>${franjaHorariaTexto}</td>
                <td><span class="badge ${estadoClass}">${estadoTexto}</span></td>
            `;
            citasTableBody.appendChild(row);
        }
    };
    
    // Manejar el evento de cambio en el input de fecha (simulando un filtro)
    filtroFechaInput.addEventListener('change', async (e) => {
        const fechaSeleccionada = e.target.value;
        if (fechaSeleccionada) {
            // Aquí puedes implementar la lógica de búsqueda por fecha si tu API lo soporta.
            // Para este ejemplo, se buscará por el ID de la cita.
            console.log(`Buscando citas para la fecha: ${fechaSeleccionada}`);
            // Simulación de búsqueda por ID, si el input fuera un campo de texto para ID
            const idToSearch = '123'; // Reemplaza esto con un valor real
            const cita = await fetchCitaById(idToSearch);
            if (cita) {
                renderCitasTable([cita]);
            } else {
                citasTableBody.innerHTML = `<tr><td colspan="6" class="text-center text-muted">No se encontró la cita con el ID ${idToSearch}.</td></tr>`;
            }
        } else {
            fetchAllCitas();
        }
    });

    // Cargar todas las citas al iniciar
    fetchAllCitas();
});
