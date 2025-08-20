document.addEventListener('DOMContentLoaded', () => {
    // URL de tu API para los servicios
    const API_URL = 'https://localhost:7172/api/Servicio';

    // Elementos del DOM que vamos a manipular
    const tableBody = document.getElementById('servicios-table-body');
    const form = document.getElementById('servicio-form');
    const servicioNombreInput = document.getElementById('servicio-nombre');
    const servicioPrecioInput = document.getElementById('servicio-precio');
    const servicioIdInput = document.getElementById('servicio-id'); // Campo oculto para el ID
    const formTitle = document.getElementById('form-title');
    const formButtonText = document.getElementById('form-button-text');

    /**
     * Renderiza la lista de servicios en la tabla.
     * @param {Array} servicios - Array de objetos de servicio.
     */
    const renderServicios = (servicios) => {
        tableBody.innerHTML = ''; // Limpiar el contenido actual de la tabla
        servicios.forEach(servicio => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${servicio.nombre}</td>
                <td>$${servicio.precio.toFixed(2)}</td>
                <td>
                    <button class="btn btn-sm btn-outline-warning edit-btn" data-id="${servicio.idServicio}" data-nombre="${servicio.nombre}" data-precio="${servicio.precio}">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger delete-btn" data-id="${servicio.idServicio}">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    };

    /**
     * Obtiene la lista de servicios desde la API.
     */
    const fetchServicios = async () => {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const servicios = await response.json();
            renderServicios(servicios);
        } catch (error) {
            console.error('Error fetching servicios:', error);
            tableBody.innerHTML = '<tr><td colspan="3" class="text-danger text-center">Error al cargar los servicios.</td></tr>';
        }
    };

    /**
     * Maneja el envío del formulario para crear o editar un servicio.
     */
    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        const servicioNombre = servicioNombreInput.value;
        const servicioPrecio = parseFloat(servicioPrecioInput.value);
        const servicioId = servicioIdInput.value;

        // Validar que el precio es un número válido
        if (isNaN(servicioPrecio)) {
            alert('Por favor, ingrese un precio válido.');
            return;
        }

        if (servicioId) {
            // Editar (PUT)
            const servicioData = { idServicio: servicioId, nombre: servicioNombre, precio: servicioPrecio };
            try {
                const response = await fetch(`${API_URL}/${servicioId}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(servicioData),
                });
                if (!response.ok) throw new Error('Failed to update servicio');
                console.log('Servicio actualizado con éxito');
            } catch (error) {
                console.error('Error updating servicio:', error);
            }
        } else {
            // Crear (POST)
            const servicioData = { nombre: servicioNombre, precio: servicioPrecio };
            try {
                const response = await fetch(API_URL, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(servicioData),
                });
                if (!response.ok) throw new Error('Failed to create servicio');
                console.log('Servicio creado con éxito');
            } catch (error) {
                console.error('Error creating servicio:', error);
            }
        }
        
        // Limpiar el formulario y recargar la tabla
        form.reset();
        servicioIdInput.value = '';
        formTitle.textContent = 'Nuevo Servicio';
        formButtonText.textContent = 'Guardar';
        fetchServicios();
    });

    /**
     * Maneja los clics en los botones de la tabla para editar y eliminar.
     */
    tableBody.addEventListener('click', async (e) => {
        const target = e.target.closest('button');
        if (!target) return;

        if (target.classList.contains('edit-btn')) {
            // Cargar datos en el formulario para editar
            const servicioId = target.dataset.id;
            const servicioNombre = target.dataset.nombre;
            const servicioPrecio = target.dataset.precio;
            
            servicioIdInput.value = servicioId;
            servicioNombreInput.value = servicioNombre;
            servicioPrecioInput.value = servicioPrecio;
            formTitle.textContent = 'Editar Servicio';
            formButtonText.textContent = 'Actualizar';
        } else if (target.classList.contains('delete-btn')) {
            // Eliminar (DELETE)
            const servicioId = target.dataset.id;
            if (confirm('¿Está seguro de que desea eliminar este servicio?')) {
                try {
                    const response = await fetch(`${API_URL}/${servicioId}`, {
                        method: 'DELETE',
                    });
                    if (!response.ok) throw new Error('Failed to delete servicio');
                    console.log('Servicio eliminado con éxito');
                    fetchServicios(); // Recargar la tabla
                } catch (error) {
                    console.error('Error deleting servicio:', error);
                }
            }
        }
    });

    // Cargar los servicios al iniciar la página
    fetchServicios();
});
