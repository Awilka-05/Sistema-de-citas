document.addEventListener('DOMContentLoaded', () => {
    const API_URL = 'https://localhost:7172/api/Lugar';
    const tableBody = document.getElementById('lugares-table-body');
    const form = document.getElementById('lugar-form');
    const lugarNombreInput = document.getElementById('lugar-nombre');
    const lugarIdInput = document.getElementById('lugar-id');
    const formTitle = document.getElementById('form-title');
    const formButtonText = document.getElementById('form-button-text');

    // Función para renderizar los lugares en la tabla
    const renderLugares = (lugares) => {
        tableBody.innerHTML = '';
        lugares.forEach(lugar => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${lugar.nombre}</td>
                <td>
                    <button class="btn btn-sm btn-outline-warning edit-btn" data-id="${lugar.idLugar}" data-nombre="${lugar.nombre}">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger delete-btn" data-id="${lugar.idLugar}">
                        <i class="fas fa-trash"></i>
                    </button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    };

    // Función para obtener los lugares de la API
    const fetchLugares = async () => {
        try {
            const response = await fetch(API_URL);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const lugares = await response.json();
            renderLugares(lugares);
        } catch (error) {
            console.error('Error fetching lugares:', error);
        }
    };

    // Manejar el envío del formulario para crear o editar
    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        const lugarNombre = lugarNombreInput.value;
        const lugarId = lugarIdInput.value;

        if (lugarId) {
            // Editar (PUT)
            const lugarData = { idLugar: lugarId, nombre: lugarNombre };
            try {
                const response = await fetch(`${API_URL}/${lugarId}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(lugarData),
                });
                if (!response.ok) {
                    throw new Error('Failed to update lugar');
                }
                console.log('Lugar actualizado con éxito');
            } catch (error) {
                console.error('Error updating lugar:', error);
            }
        } else {
            // Crear (POST)
            const lugarData = { nombre: lugarNombre };
            try {
                const response = await fetch(API_URL, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(lugarData),
                });
                if (!response.ok) {
                    throw new Error('Failed to create lugar');
                }
                console.log('Lugar creado con éxito');
            } catch (error) {
                console.error('Error creating lugar:', error);
            }
        }
        
        // Limpiar el formulario y volver al modo de creación
        lugarNombreInput.value = '';
        lugarIdInput.value = '';
        formTitle.textContent = 'Nuevo Lugar';
        formButtonText.textContent = 'Guardar';
        fetchLugares(); // Recargar la lista de lugares
    });

    // Manejar clics en la tabla para editar y eliminar
    tableBody.addEventListener('click', async (e) => {
        // Eliminar (DELETE)
        if (e.target.closest('.delete-btn')) {
            const lugarId = e.target.closest('.delete-btn').dataset.id;
            if (confirm('¿Está seguro de que desea eliminar este lugar?')) {
                try {
                    const response = await fetch(`${API_URL}/${lugarId}`, {
                        method: 'DELETE',
                    });
                    if (!response.ok) {
                        throw new Error('Failed to delete lugar');
                    }
                    console.log('Lugar eliminado con éxito');
                    fetchLugares(); // Recargar la lista
                } catch (error) {
                    console.error('Error deleting lugar:', error);
                }
            }
        }

        // Editar (PUT) - Cargar datos en el formulario
        if (e.target.closest('.edit-btn')) {
            const editBtn = e.target.closest('.edit-btn');
            const lugarId = editBtn.dataset.id;
            const lugarNombre = editBtn.dataset.nombre;
            
            lugarNombreInput.value = lugarNombre;
            lugarIdInput.value = lugarId;
            formTitle.textContent = 'Editar Lugar';
            formButtonText.textContent = 'Actualizar';
        }
    });

    // Cargar los lugares al iniciar
    fetchLugares();
});
