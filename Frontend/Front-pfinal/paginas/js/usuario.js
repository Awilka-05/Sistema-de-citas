let selectedSlot = null;
let selectedService = '';
let selectedLocation = '';
let currentWeekStart = new Date();
let userHasAppointment = false;

// Datos simulados de citas (normalmente vendría del backend)
const appointmentData = {
    service: 'Renovación de Licencia',
    location: 'Oficina Central',
    date: '2025-08-22',
    time: '10:00 AM'
};

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    initializeUser();
    setupEventListeners();
    setCurrentWeek();
});

function initializeUser() {
    // Verificar si el usuario tiene cita activa (simulado)
    checkExistingAppointment();
    
    // Configurar semana actual
    updateWeekDisplay();
}

function setupEventListeners() {
    // Selectores de servicio y lugar
    const servicioSelect = document.getElementById('servicio-select');
    const lugarSelect = document.getElementById('lugar-select');
    
    if (servicioSelect) {
        servicioSelect.addEventListener('change', function() {
            selectedService = this.value;
            updateCalendarVisibility();
            clearTimeSlotSelection();
        });
    }
    
    if (lugarSelect) {
        lugarSelect.addEventListener('change', function() {
            selectedLocation = this.value;
            updateCalendarVisibility();
            clearTimeSlotSelection();
        });
    }

    // Navegación de semanas
    const prevWeekBtn = document.getElementById('prev-week');
    const nextWeekBtn = document.getElementById('next-week');
    
    if (prevWeekBtn) {
        prevWeekBtn.addEventListener('click', function() {
            changeWeek(-7);
        });
    }
    
    if (nextWeekBtn) {
        nextWeekBtn.addEventListener('click', function() {
            changeWeek(7);
        });
    }

    // Slots de tiempo
    document.querySelectorAll('.time-slot.available').forEach(slot => {
        slot.addEventListener('click', function() {
            selectTimeSlot(this);
        });
    });

    // Modal de confirmación
    const btnConfirmar = document.getElementById('btn-confirmar');
    if (btnConfirmar) {
        btnConfirmar.addEventListener('click', confirmarCita);
    }

    // Modal de cancelación
    const btnCancelarConfirmar = document.getElementById('btn-cancelar-confirmar');
    if (btnCancelarConfirmar) {
        btnCancelarConfirmar.addEventListener('click', confirmarCancelacion);
    }
}

function checkExistingAppointment() {
    // Simular verificación de cita existente
    // En una aplicación real, esto vendría del backend
    const hasAppointment = false; // Cambiar a true para probar con cita existente
    
    if (hasAppointment) {
        showExistingAppointment(appointmentData);
        userHasAppointment = true;
    }
}

function showExistingAppointment(appointment) {
    const citaActual = document.getElementById('cita-actual');
    const servicioActual = document.getElementById('servicio-actual');
    const fechaActual = document.getElementById('fecha-actual');
    const lugarActual = document.getElementById('lugar-actual');
    
    if (citaActual && servicioActual && fechaActual && lugarActual) {
        servicioActual.textContent = appointment.service;
        fechaActual.textContent = formatDate(appointment.date) + ' a las ' + appointment.time;
        lugarActual.textContent = appointment.location;
        citaActual.style.display = 'block';
    }
}

function setCurrentWeek() {
    const today = new Date();
    const dayOfWeek = today.getDay();
    const monday = new Date(today);
    
    // Ajustar para obtener el lunes de esta semana
    const daysUntilMonday = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
    monday.setDate(today.getDate() + daysUntilMonday);
    
    currentWeekStart = monday;
    updateWeekDisplay();
}

function updateWeekDisplay() {
    const weekDisplay = document.querySelector('.week-display');
    if (weekDisplay) {
        const startDate = new Date(currentWeekStart);
        const endDate = new Date(currentWeekStart);
        endDate.setDate(startDate.getDate() + 4); // Viernes
        
        const formatOptions = { day: 'numeric', month: 'short' };
        const startStr = startDate.toLocaleDateString('es-ES', formatOptions);
        const endStr = endDate.toLocaleDateString('es-ES', formatOptions);
        const year = startDate.getFullYear();
        
        weekDisplay.textContent = `${startStr} - ${endStr} ${year}`;
    }
    
    updateCalendarDates();
}

function updateCalendarDates() {
    const dayHeaders = document.querySelectorAll('.day-header');
    const days = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes'];
    
    dayHeaders.forEach((header, index) => {
        const date = new Date(currentWeekStart);
        date.setDate(currentWeekStart.getDate() + index);
        
        const dayName = header.querySelector('h6');
        const dayDate = header.querySelector('small');
        
        if (dayName && dayDate) {
            dayName.textContent = days[index];
            dayDate.textContent = date.toLocaleDateString('es-ES', { 
                day: '2-digit', 
                month: '2-digit' 
            });
        }
    });
    
    // Actualizar atributos de fecha en los slots
    updateTimeSlotDates();
}

function updateTimeSlotDates() {
    const daySlots = document.querySelectorAll('.day-slots');
    
    daySlots.forEach((daySlot, dayIndex) => {
        const date = new Date(currentWeekStart);
        date.setDate(currentWeekStart.getDate() + dayIndex);
        const dateStr = date.toISOString().split('T')[0];
        
        const slots = daySlot.querySelectorAll('.time-slot');
        slots.forEach(slot => {
            if (!slot.disabled) {
                slot.setAttribute('data-date', dateStr);
            }
        });
    });
}

function changeWeek(days) {
    const newDate = new Date(currentWeekStart);
    newDate.setDate(currentWeekStart.getDate() + days);
    
    // No permitir fechas anteriores a hoy
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    if (newDate >= today || days > 0) {
        currentWeekStart = newDate;
        updateWeekDisplay();
        clearTimeSlotSelection();
    }
}

function updateCalendarVisibility() {
    const calendarPlaceholder = document.querySelector('.calendar-placeholder');
    const calendarGrid = document.getElementById('calendar-grid');
    
    if (selectedService && selectedLocation) {
        calendarPlaceholder.style.display = 'none';
        calendarGrid.style.display = 'block';
    } else {
        calendarPlaceholder.style.display = 'block';
        calendarGrid.style.display = 'none';
    }
}

function selectTimeSlot(slot) {
    if (userHasAppointment) {
        alert('Ya tienes una cita agendada. Debes cancelarla antes de agendar una nueva.');
        return;
    }

    // Remover selección anterior
    document.querySelectorAll('.time-slot.selected').forEach(s => {
        s.classList.remove('selected');
    });
    
    // Seleccionar nuevo slot
    slot.classList.add('selected');
    selectedSlot = {
        time: slot.getAttribute('data-time'),
        date: slot.getAttribute('data-date'),
        element: slot
    };
    
    // Mostrar modal de confirmación
    showConfirmationModal();
}

function clearTimeSlotSelection() {
    document.querySelectorAll('.time-slot.selected').forEach(s => {
        s.classList.remove('selected');
    });
    selectedSlot = null;
}

function showConfirmationModal() {
    if (!selectedSlot || !selectedService || !selectedLocation) return;
    
    // Obtener nombres legibles
    const serviceName = getServiceName(selectedService);
    const locationName = getLocationName(selectedLocation);
    const formattedDate = formatDate(selectedSlot.date);
    
    // Llenar modal
    document.getElementById('confirm-servicio').textContent = serviceName;
    document.getElementById('confirm-lugar').textContent = locationName;
    document.getElementById('confirm-fecha').textContent = formattedDate;
    document.getElementById('confirm-hora').textContent = selectedSlot.time;
    
    // Mostrar modal
    const modal = new bootstrap.Modal(document.getElementById('confirmModal'));
    modal.show();
}

function getServiceName(value) {
    const services = {
        'renovacion-licencia': 'Renovación de Licencia',
        'registro-vehicular': 'Registro Vehicular',
        'inspeccion': 'Inspección Vehicular'
    };
    return services[value] || value;
}

function getLocationName(value) {
    const locations = {
        'oficina-central': 'Oficina Central',
        'oficina-norte': 'Oficina Norte',
        'oficina-sur': 'Oficina Sur'
    };
    return locations[value] || value;
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

function confirmarCita() {
    // Simular confirmación de cita
    const citaData = {
        service: getServiceName(selectedService),
        location: getLocationName(selectedLocation),
        date: selectedSlot.date,
        time: selectedSlot.time
    };
    
    // Mostrar cita confirmada
    showExistingAppointment(citaData);
    userHasAppointment = true;
    
    // Marcar slot como ocupado
    selectedSlot.element.classList.remove('available', 'selected');
    selectedSlot.element.classList.add('occupied');
    selectedSlot.element.disabled = true;
    selectedSlot.element.textContent = selectedSlot.time;
    
    // Cerrar modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('confirmModal'));
    modal.hide();
    
    // Limpiar selección
    selectedSlot = null;
    
    // Resetear selectores
    document.getElementById('servicio-select').value = '';
    document.getElementById('lugar-select').value = '';
    selectedService = '';
    selectedLocation = '';
    updateCalendarVisibility();
    
    // Mostrar mensaje de éxito
    const toast = document.createElement('div');
    toast.className = 'toast-container position-fixed top-0 end-0 p-3';
    toast.innerHTML = `
        <div class="toast show" role="alert">
            <div class="toast-header">
                <i class="fas fa-check-circle text-success me-2"></i>
                <strong class="me-auto">Cita Confirmada</strong>
            </div>
            <div class="toast-body">
                Su cita ha sido agendada exitosamente.
            </div>
        </div>
    `;
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.remove();
    }, 5000);
}

function cancelarCita() {
    const modal = new bootstrap.Modal(document.getElementById('cancelModal'));
    modal.show();
}

function confirmarCancelacion() {
    // Ocultar cita actual
    const citaActual = document.getElementById('cita-actual');
    if (citaActual) {
        citaActual.style.display = 'none';
    }
    
    userHasAppointment = false;
    
    // Cerrar modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('cancelModal'));
    modal.hide();
    
    // Mostrar mensaje
    alert('Su cita ha sido cancelada exitosamente.');
}

// Funciones de utilidad
function showToast(message, type = 'success') {
    const toastContainer = document.createElement('div');
    toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
    
    const iconClass = type === 'success' ? 'fa-check-circle text-success' : 'fa-exclamation-triangle text-warning';
    
    toastContainer.innerHTML = `
        <div class="toast show" role="alert">
            <div class="toast-header">
                <i class="fas ${iconClass} me-2"></i>
                <strong class="me-auto">INTRANT</strong>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;
    
    document.body.appendChild(toastContainer);
    
    setTimeout(() => {
        toastContainer.remove();
    }, 5000);
}