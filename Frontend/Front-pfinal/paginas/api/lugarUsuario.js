document.addEventListener('DOMContentLoaded', () => {
    // La URL de tu API para obtener los lugares.
    const API_URL = 'https://localhost:7172/api/Lugar';

    // Elementos del DOM que vamos a manipular.
    const lugarSelect = document.getElementById('lugar-select');

    // Variable para almacenar el ID del lugar seleccionado.
    let selectedLugarId = null;

    /**
     * Rellena el elemento <select> con los lugares obtenidos de la API.
     * @param {Array} lugares - Un array de objetos de lugares.
     */
    const renderLugares = (lugares) => {
        // Limpiar las opciones existentes, manteniendo la primera por defecto.
        lugarSelect.innerHTML = '<option value="">Seleccione un lugar</option>';
        
        // Iterar sobre los datos y crear una nueva opción para cada lugar.
        lugares.forEach(lugar => {
            const option = document.createElement('option');
            // El valor de la opción será el ID del lugar, que es lo que espera tu API.
            option.value = lugar.idLugar;
            // El texto de la opción será el nombre del lugar.
            option.textContent = lugar.nombre;
            lugarSelect.appendChild(option);
        });
    };

    /**
     * Obtiene la lista de lugares desde la API.
     */
    const fetchLugares = async () => {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) {
                // Manejo de errores si la respuesta no es exitosa.
                throw new Error('Network response was not ok');
            }
            const lugares = await response.json();
            renderLugares(lugares);
        } catch (error) {
            // Si hay un error en la petición, lo registramos en la consola.
            console.error('Error fetching lugares:', error);
            // También puedes mostrar un mensaje al usuario en el <select>.
            lugarSelect.innerHTML = '<option value="" class="text-danger">Error al cargar</option>';
        }
    };

    /**
     * Maneja el evento de cambio en el elemento <select>.
     */
    lugarSelect.addEventListener('change', (e) => {
        // Almacenar el ID del lugar seleccionado.
        // Si el usuario selecciona la opción por defecto, el valor será una cadena vacía.
        selectedLugarId = e.target.value;

        // Puedes usar esta variable para la próxima acción, como hacer otra llamada a la API
        // con el ID del lugar para obtener los servicios, horarios, etc.
        console.log(`Lugar seleccionado con ID: ${selectedLugarId}`);
        
        // Ejemplo: Si quieres hacer algo solo cuando se selecciona un lugar válido
        if (selectedLugarId) {
            // Aquí iría el código para la siguiente etapa, por ejemplo:
            // fetchServiciosPorLugar(selectedLugarId);
        }
    });

    // Llama a la función para cargar los lugares cuando el documento esté listo.
    fetchLugares();
});
