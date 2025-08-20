document.addEventListener('DOMContentLoaded', () => {
    // API URL definitions
    const API_URL_HORARIO = 'https://localhost:7172/api/Horario';
    const API_URL_CONFIGURACION = 'https://localhost:7172/api/ConfiguracionTurno';

    // DOM elements
    const listaConfiguraciones = document.getElementById('lista-configuraciones');
    const formConfiguracion = document.getElementById('form-configuracion');
    const loadingMessage = document.getElementById('loading-message');

    const fetchAndRenderConfiguraciones = async () => {
        listaConfiguraciones.innerHTML = '';
        loadingMessage.style.display = 'block';

        try {
            const response = await fetch(API_URL_CONFIGURACION);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const configuraciones = await response.json();

            loadingMessage.style.display = 'none';

            if (configuraciones.length === 0) {
                listaConfiguraciones.innerHTML = '<div class="text-center text-muted">No hay configuraciones activas.</div>';
                return;
            }

            configuraciones.forEach(config => {
                const configItem = document.createElement('div');
                configItem.classList.add('configuracion-item');
                configItem.innerHTML = `
                    <div class="config-info">
                        <h6>Configuración del Turno #${config.turnoId}</h6>
                        <p><strong>Fechas:</strong> ${new Date(config.fechaInicio).toLocaleDateString()} - ${new Date(config.fechaFin).toLocaleDateString()}</p>
                        <p><strong>Horario:</strong> ${config.horario.horaInicio} - ${config.horario.horaFin}</p>
                        <p><strong>Descripción:</strong> ${config.horario.descripcion}</p>
                        <p><strong>Duración:</strong> ${config.duracionMinutos} minutos</p>
                        <p><strong>Estaciones:</strong> ${config.cantidadEstaciones}</p>
                    </div>
                    <div class="config-actions">
                        <button class="btn btn-sm btn-outline-danger delete-btn" 
                                data-configuracion-id="${config.turnoId}" 
                                data-horario-id="${config.horariosId}">
                            <i class="fas fa-trash"></i> Eliminar
                        </button>
                    </div>
                `;
                listaConfiguraciones.appendChild(configItem);
            });
        } catch (error) {
            console.error('Error fetching configuraciones:', error);
            loadingMessage.style.display = 'none';
            listaConfiguraciones.innerHTML = `<div class="text-center text-danger">Error al cargar las configuraciones</div>`;
        }
    };

    const handleFormSubmit = async (e) => {
        e.preventDefault();

        const fechaInicio = document.getElementById('fecha-inicio').value;
        const fechaFin = document.getElementById('fecha-fin').value;
        const horaInicio = document.getElementById('hora-inicio').value;
        const horaFin = document.getElementById('hora-fin').value;
        const duracionCita = document.getElementById('duracion-cita').value;
        const estacionesDisponibles = document.getElementById('estaciones-disponibles').value;
        const descripcionConfig = document.getElementById('descripcion-config').value;

        const horarioData = {
            horaInicio: horaInicio,
            horaFin: horaFin,
            descripcion: descripcionConfig
        };

        try {
            const responseHorario = await fetch(API_URL_HORARIO, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(horarioData),
            });

            if (!responseHorario.ok) {
                throw new Error(`Error al crear el horario. Status: ${responseHorario.status}`);
            }
            

            const nuevoHorario = await responseHorario.json();
            const nuevoHorarioId = nuevoHorario.horarioId;

            const configuracionData = {
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                horariosId: nuevoHorarioId,
                cantidadEstaciones: parseInt(estacionesDisponibles),
                duracionMinutos: parseInt(duracionCita)
            };

            const responseConfiguracion = await fetch(API_URL_CONFIGURACION, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(configuracionData),
            });

           const data = await responseConfiguracion.json();

            if (!responseConfiguracion.ok || data.success === false) {
                alert(data.message || `Error al crear la configuración. Status: ${responseConfiguracion.status}`);
                return; 
            }
            

            formConfiguracion.reset();
            alert('Configuración guardada con éxito.');
            fetchAndRenderConfiguraciones();

        } catch (error) {
            console.error('Error al guardar la configuración:', error);
            alert('Hubo un error al guardar la configuración');
        }
    };

 
    formConfiguracion.addEventListener('submit', handleFormSubmit);
    

    fetchAndRenderConfiguraciones();
});
