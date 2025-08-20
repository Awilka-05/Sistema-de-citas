document.addEventListener('DOMContentLoaded', () => {
    // API URLs
    const API_URL_CITAS = 'https://localhost:7172/api/Cita';
    const API_URL_SERVICIOS = 'https://localhost:7172/api/Servicio';
    const API_URL_LUGARES = 'https://localhost:7172/api/Lugar';
    const API_URL_CONFIGURACION = 'https://localhost:7172/api/ConfiguracionTurno';

    // DOM elements
    const serviciosList = document.getElementById('servicios-list');
    const lugaresSelect = document.getElementById('lugar-select');
    const calendarGrid = document.getElementById('calendar-grid');
    const confirmModal = new bootstrap.Modal(document.getElementById('confirmModal'));
    const btnConfirmar = document.getElementById('btn-confirmar');
    const confirmServicio = document.getElementById('confirm-servicio');
    const confirmLugar = document.getElementById('confirm-lugar');
    const confirmFecha = document.getElementById('confirm-fecha');
    const confirmHora = document.getElementById('confirm-hora');
    const citaActual = document.getElementById('cita-actual');
    const servicioActual = document.getElementById('servicio-actual');
    const fechaActual = document.getElementById('fecha-actual');
    const lugarActual = document.getElementById('lugar-actual');

    // State variables
    let selectedServiceId = null;
    let selectedLugarId = null;
    let selectedFranjaId = null;
    let selectedTurnoId = null;
    let selectedDate = null;

    let servicios = [];
    let lugares = [];
    let configuraciones = [];

    // Helper function to get the first day of the week (Monday)
    const getStartOfWeek = (date) => {
        const d = new Date(date);
        const day = d.getDay();
        const diff = d.getDate() - day + (day === 0 ? -6 : 1); // Adjust for Monday as start of week
        return new Date(d.setDate(diff));
    };

    // Helper function to format date
    const formatDate = (date) => {
        const d = new Date(date);
        return d.toLocaleDateString('es-ES', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    };

    // Function to fetch all necessary data
    const fetchData = async () => {
        try {
            const [serviciosRes, lugaresRes, configuracionesRes] = await Promise.all([
                fetch(API_URL_SERVICIOS),
                fetch(API_URL_LUGARES),
                fetch(API_URL_CONFIGURACION)
            ]);
            servicios = await serviciosRes.json();
            lugares = await lugaresRes.json();
            configuraciones = await configuracionesRes.json();
            
            renderServicios();
            renderLugares();
            renderCalendar(new Date());

            // Check and display existing appointment
            checkExistingAppointment();
        } catch (error) {
            console.error('Error fetching data:', error);
            // Show an error message to the user
            document.querySelector('.container').innerHTML = `<div class="alert alert-danger text-center">Error al cargar la aplicación. Por favor, intente de nuevo más tarde.</div>`;
        }
    };

    // Function to render services
    const renderServicios = () => {
        serviciosList.innerHTML = '';
        servicios.forEach(servicio => {
            const button = document.createElement('button');
            button.classList.add('btn', 'btn-outline-primary', 'w-100', 'mb-2');
            button.textContent = servicio.nombre;
            button.dataset.id = servicio.servicioId;
            button.addEventListener('click', () => {
                // Remove active class from all buttons
                document.querySelectorAll('.btn-outline-primary').forEach(btn => btn.classList.remove('active'));
                // Add active class to the clicked button
                button.classList.add('active');
                selectedServiceId = servicio.servicioId;
            });
            serviciosList.appendChild(button);
        });
    };

    // Function to render places
    const renderLugares = () => {
        lugaresSelect.innerHTML = '<option value="">Seleccione un lugar</option>';
        lugares.forEach(lugar => {
            const option = document.createElement('option');
            option.value = lugar.lugarId;
            option.textContent = lugar.nombre;
            lugaresSelect.appendChild(option);
        });
    };

    // Function to render the calendar week
    const renderCalendar = (date) => {
        // ... (existing calendar rendering logic)
        const startOfWeek = getStartOfWeek(date);
        const endOfWeek = new Date(startOfWeek);
        endOfWeek.setDate(endOfWeek.getDate() + 6);

        // Update the week display
        const startDay = startOfWeek.getDate();
        const endDay = endOfWeek.getDate();
        const startMonth = startOfWeek.toLocaleString('es-ES', { month: 'long' });
        const endMonth = endOfWeek.toLocaleString('es-ES', { month: 'long' });
        document.getElementById('week-display').textContent = `${startDay} ${startMonth} - ${endDay} ${endMonth} ${startOfWeek.getFullYear()}`;

        // Clear previous content
        calendarGrid.innerHTML = '';

        const daysOfWeek = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

        for (let i = 0; i < 7; i++) {
            const currentDay = new Date(startOfWeek);
            currentDay.setDate(currentDay.getDate() + i);

            const dayColumn = document.createElement('div');
            dayColumn.classList.add('day-column');
            dayColumn.innerHTML = `
                <div class="day-header">
                    <h6>${daysOfWeek[i]}</h6>
                    <small>${currentDay.getDate()}/${currentDay.getMonth() + 1}</small>
                </div>
            `;
            
            const daySlotsContainer = document.createElement('div');
            daySlotsContainer.classList.add('day-slots');
            daySlotsContainer.dataset.date = formatDate(currentDay);

            const dayConfigurations = configuraciones.filter(config => {
                const configStart = new Date(config.fechaInicio);
                const configEnd = new Date(config.fechaFin);
                return currentDay >= configStart && currentDay <= configEnd;
            });

            if (dayConfigurations.length > 0) {
                 dayConfigurations.forEach(config => {
                    if (config.franjas && config.franjas.length > 0) {
                        config.franjas.forEach(franja => {
                            const timeSlotBtn = document.createElement('button');
                            timeSlotBtn.classList.add('time-slot');
                            timeSlotBtn.textContent = `${franja.horaInicio.substring(0, 5)}`;
                            timeSlotBtn.dataset.franjaId = franja.franjaId;
                            timeSlotBtn.dataset.turnoId = franja.configuracionTurnoId;
                            
                            const isOccupied = franja.horaInicio.substring(0, 2) === '10';
                            
                            if (isOccupied) {
                                timeSlotBtn.classList.add('occupied');
                                timeSlotBtn.disabled = true;
                            } else {
                                timeSlotBtn.classList.add('available');
                            }
                            
                            if (selectedFranjaId === franja.franjaId) {
                                timeSlotBtn.classList.add('selected');
                            }
                            
                            daySlotsContainer.appendChild(timeSlotBtn);
                        });
                    }
                });
            } else {
                 daySlotsContainer.innerHTML = `<small class="text-muted">No hay turnos</small>`;
            }

            dayColumn.appendChild(daySlotsContainer);
            calendarGrid.appendChild(dayColumn);
        }
    };

    // Function to handle time slot selection
    const handleSlotClick = (e) => {
        const target = e.target.closest('.time-slot');
        if (!target || target.disabled) return;
        
        // Remove 'selected' class from all other slots
        document.querySelectorAll('.time-slot.selected').forEach(slot => {
            slot.classList.remove('selected');
            slot.classList.add('available');
        });

        // Add 'selected' class to the clicked slot
        target.classList.add('selected');
        selectedFranjaId = target.dataset.franjaId;
        selectedTurnoId = target.dataset.turnoId;
        
        // Get the date from the parent container
        const parentContainer = target.closest('.day-slots');
        if (parentContainer) {
            selectedDate = parentContainer.dataset.date;
        }

        // Show confirmation modal
        showConfirmationModal();
    };

    // Function to get a service by its ID
    const getServicioById = (id) => servicios.find(s => s.servicioId === parseInt(id));

    // Function to get a place by its ID
    const getLugarById = (id) => lugares.find(l => l.lugarId === parseInt(id));

    // Function to get a franja horaria by its ID
    const getFranjaById = (turnoId, franjaId) => {
        const configuracion = configuraciones.find(c => c.turnoId === parseInt(turnoId));
        if (configuracion && configuracion.franjas) {
            return configuracion.franjas.find(f => f.franjaId === parseInt(franjaId));
        }
        return null;
    };

    // Function to show the confirmation modal
    const showConfirmationModal = () => {
        if (!selectedServiceId || !selectedLugarId || !selectedFranjaId || !selectedTurnoId) {
            // Do not show modal if a required field is missing
            return;
        }

        const servicio = getServicioById(selectedServiceId);
        const lugar = getLugarById(selectedLugarId);
        const franja = getFranjaById(selectedTurnoId, selectedFranjaId);

        confirmServicio.textContent = servicio ? servicio.nombre : 'N/A';
        confirmLugar.textContent = lugar ? lugar.nombre : 'N/A';
        confirmFecha.textContent = selectedDate ? selectedDate : 'N/A';
        confirmHora.textContent = franja ? `${franja.horaInicio.substring(0, 5)} - ${franja.horaFin.substring(0, 5)}` : 'N/A';

        confirmModal.show();
    };

    // Function to handle the confirmation button click
    const handleConfirm = async () => {
        // Create the cita object
        const newCita = {
            idUsuario: 'usuario-ejemplo-1', // Replace with a real user ID
            fechaCita: new Date(selectedDate),
            franjaId: parseInt(selectedFranjaId),
            turnoId: parseInt(selectedTurnoId),
            lugarId: parseInt(selectedLugarId),
            servicioId: parseInt(selectedServiceId),
            estado: 1 // 1 = Confirmada
        };

        try {
            const response = await fetch(API_URL_CITAS, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newCita)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const result = await response.json();
            console.log('Cita creada exitosamente:', result);
            alert('Cita confirmada con éxito!');
            confirmModal.hide();
            
            // Check and display the newly created appointment
            checkExistingAppointment();
        } catch (error) {
            console.error('Error al crear la cita:', error);
            alert('Error al confirmar la cita. Intente de nuevo.');
        }
    };

    // Function to check and display the existing appointment
    const checkExistingAppointment = async () => {
        try {
            // This is a simplified check. In a real app, you would fetch the appointment for the logged-in user.
            const response = await fetch(`${API_URL_CITAS}/usuario/1`); // Assuming an endpoint to get citas by user ID
            if (response.ok) {
                const cita = await response.json();
                if (cita && cita.estado === 1) { // Check if a confirmed cita exists
                    const servicio = getServicioById(cita.servicioId);
                    const lugar = getLugarById(cita.lugarId);
                    const configuracion = await fetchConfiguracionById(cita.turnoId);
                    
                    let franjaHorariaTexto = 'N/A';
                    if (configuracion && configuracion.franjas) {
                        const franja = configuracion.franjas.find(f => f.franjaId === cita.franjaId);
                        if (franja) {
                            franjaHorariaTexto = `${franja.horaInicio.substring(0, 5)} - ${franja.horaFin.substring(0, 5)}`;
                        }
                    }

                    servicioActual.textContent = servicio ? servicio.nombre : 'N/A';
                    fechaActual.textContent = `${new Date(cita.fechaCita).toLocaleDateString()} a las ${franjaHorariaTexto}`;
                    lugarActual.textContent = lugar ? lugar.nombre : 'N/A';
                    citaActual.style.display = 'block';
                } else {
                    citaActual.style.display = 'none';
                }
            } else {
                citaActual.style.display = 'none';
            }
        } catch (error) {
            console.error('Error checking for existing appointment:', error);
            citaActual.style.display = 'none';
        }
    };

    // Event listeners
    lugaresSelect.addEventListener('change', (e) => {
        selectedLugarId = e.target.value;
    });

    calendarGrid.addEventListener('click', handleSlotClick);
    btnConfirmar.addEventListener('click', handleConfirm);
    
    // Initial data fetch
    fetchData();
});

// Assuming a simple function to handle cancellation
const cancelarCita = () => {
    // Implement cancellation logic here
    alert('Función de cancelar cita no implementada en este ejemplo.');
};
