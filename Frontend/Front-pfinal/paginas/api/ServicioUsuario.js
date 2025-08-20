document.addEventListener('DOMContentLoaded', () => {
    // URL de tu API para los servicios
    const API_URL = 'https://localhost:7172/api/Servicio';

    // Elemento del DOM que vamos a manipular: el select para los servicios
    const servicioSelect = document.getElementById('servicio-select');
    let selectedServicioId = null;

    /**
     * Obtiene la lista de servicios desde la API y la renderiza en el select.
     */
    const fetchServicios = async () => {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) {
                // Si la respuesta no es exitosa, se lanza un error.
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const servicios = await response.json();
            
            // Llenar el select con los servicios
            servicioSelect.innerHTML = '<option value="">Seleccione un servicio</option>';
            servicios.forEach(servicio => {
                const option = document.createElement('option');
                option.value = servicio.idServicio;
                option.textContent = servicio.nombre;
                servicioSelect.appendChild(option);
            });

        } catch (error) {
            console.error('Error fetching servicios:', error);
            // Mostrar un mensaje de error claro al usuario
            servicioSelect.innerHTML = '<option value="" class="text-danger">Error al cargar</option>';
        }
    };

    /**
     * Maneja la selección en el nuevo <select>.
     */
    servicioSelect.addEventListener('change', (e) => {
        selectedServicioId = e.target.value;
        console.log(`Servicio seleccionado con ID: ${selectedServicioId}`);
        // Aquí puedes usar selectedServicioId para la siguiente acción
    });

    // Cargar los servicios al iniciar la página
    fetchServicios();
});