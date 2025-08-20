document.addEventListener('DOMContentLoaded', () => {
    // API URL definition
    const API_URL_CONFIGURACION = 'https://localhost:7172/api/ConfiguracionTurno';

    // DOM elements
    const weekDisplay = document.getElementById('week-display');
    const prevWeekBtn = document.getElementById('prev-week');
    const nextWeekBtn = document.getElementById('next-week');
    const calendarContainer = document.getElementById('calendar-container');
    const calendarGrid = document.getElementById('calendar-grid');
    const placeholder = document.getElementById('calendar-placeholder');

    let allConfigurations = [];
    let currentDate = new Date();

    // Helper function to format date to YYYY-MM-DD
    const formatDate = (date) => {
        const d = new Date(date);
        let month = '' + (d.getMonth() + 1);
        let day = '' + d.getDate();
        const year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    };

    // Helper function to get the first day of the week (Monday)
    const getStartOfWeek = (date) => {
        const d = new Date(date);
        const day = d.getDay();
        const diff = d.getDate() - day + (day === 0 ? -6 : 1); // Adjust for Monday as start of week
        return new Date(d.setDate(diff));
    };

    // Function to render the calendar week
    const renderCalendar = (date) => {
        const startOfWeek = getStartOfWeek(date);
        const endOfWeek = new Date(startOfWeek);
        endOfWeek.setDate(endOfWeek.getDate() + 6);

        // Update the week display
        const startDay = startOfWeek.getDate();
        const endDay = endOfWeek.getDate();
        const startMonth = startOfWeek.toLocaleString('es-ES', { month: 'long' });
        const endMonth = endOfWeek.toLocaleString('es-ES', { month: 'long' });
        weekDisplay.textContent = `${startDay} ${startMonth} - ${endDay} ${endMonth} ${startOfWeek.getFullYear()}`;

        // Clear previous content
        calendarGrid.innerHTML = '';
        placeholder.style.display = 'none';
        calendarGrid.style.display = 'flex';

        const daysOfWeek = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];

        // Generate day columns and time slots
        for (let i = 0; i < 7; i++) {
            const currentDay = new Date(startOfWeek);
            currentDay.setDate(currentDay.getDate() + i);

            const dayColumn = document.createElement('div');
            dayColumn.classList.add('day-column');
            dayColumn.innerHTML = `
                <div class="day-header">
                    <h6>${daysOfWeek[currentDay.getDay()]}</h6>
                    <small>${currentDay.getDate()}/${currentDay.getMonth() + 1}</small>
                </div>
            `;
            
            const daySlotsContainer = document.createElement('div');
            daySlotsContainer.classList.add('day-slots');

            const dayConfigurations = allConfigurations.filter(config => {
                const configStart = new Date(config.fechaInicio);
                const configEnd = new Date(config.fechaFin);
                return currentDay >= configStart && currentDay <= configEnd;
            });
            
            if (dayConfigurations.length > 0) {
                 dayConfigurations.forEach(config => {
                    if (config.franjas && config.franjas.length > 0) {
                        config.franjas.forEach(franja => {
                            const timeSlotBtn = document.createElement('button');
                            timeSlotBtn.classList.add('time-slot', 'btn', 'btn-outline-primary');
                            timeSlotBtn.textContent = `${franja.horaInicio.substring(0, 5)} - ${franja.horaFin.substring(0, 5)}`;
                            timeSlotBtn.dataset.franjaId = franja.franjaId;
                            timeSlotBtn.dataset.configuracionId = config.turnoId;
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

    // Function to fetch configurations
    const fetchAndRender = async () => {
        try {
            const response = await fetch(API_URL_CONFIGURACION);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            allConfigurations = await response.json();
            renderCalendar(currentDate);
        } catch (error) {
            console.error('Error fetching configurations:', error);
            placeholder.innerHTML = `<div class="text-center p-5 text-danger">Error al cargar los datos. Revise la consola.</div>`;
        }
    };

    // Event Listeners
    prevWeekBtn.addEventListener('click', () => {
        currentDate.setDate(currentDate.getDate() - 7);
        renderCalendar(currentDate);
    });

    nextWeekBtn.addEventListener('click', () => {
        currentDate.setDate(currentDate.getDate() + 7);
        renderCalendar(currentDate);
    });

    // Initial fetch
    fetchAndRender();
});
