// Variables globales
let currentSection = 'citas';

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    initializeAdmin();
    setupEventListeners();
    setupDateValidation();
});

function initializeAdmin() {
    // Mostrar sección inicial
    showSection('citas');
    
    // Establecer fecha actual en filtros
    const today = new Date().toISOString().split('T')[0];
    const filtroFecha = document.getElementById('filtro-fecha');
    if (filtroFecha) {
        filtroFecha.value = today;
    }
}

function setupEventListeners() {
    // Navegación del sidebar
    document.querySelectorAll('.nav-item').forEach(item => {
        item.addEventListener('click', function(e) {
            e.preventDefault();
            const section = this.getAttribute('data-section');
            showSection(section);
            
            // Actualizar navegación activa
            document.querySelectorAll('.nav-item').forEach(nav => nav.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Formulario de configuración
    const formConfig = document.getElementById('form-configuracion');
    if (formConfig) {
        formConfig.addEventListener('submit', handleConfigSubmit);
    }

    // Filtro de fechas
    const filtroFecha = document.getElementById('filtro-fecha');
    if (filtroFecha) {
        filtroFecha.addEventListener('change', filtrarCitasPorFecha);
    }

    // Botón nueva configuración
    const btnNuevaConfig = document.getElementById('btn-nueva-configuracion');
    if (btnNuevaConfig) {
        btnNuevaConfig.addEventListener('click', function() {
            showSection('configuracion');
            document.querySelectorAll('.nav-item').forEach(nav => nav.classList.remove('active'));
            document.querySelector('[data-section="configuracion"]').classList.add('active');
        });
    }
}

function setupDateValidation() {
    const fechaInicio = document.getElementById('fecha-inicio');
    const fechaFin = document.getElementById('fecha-fin');
    
    if (fechaInicio && fechaFin) {
        // Establecer fecha mínima como hoy
        const today = new Date().toISOString().split('T')[0];
        fechaInicio.min = today;
        fechaFin.min = today;

        fechaInicio.addEventListener('change', function() {
            const startDate = new Date(this.value);
            const minEndDate = new Date(startDate);
            const maxEndDate = new Date(startDate);
            
            // Fecha mínima para fin: mismo día que inicio
            fechaFin.min = this.value;
            
            // Fecha máxima para fin: 6 días después del inicio
            maxEndDate.setDate(startDate.getDate() + 6);
            fechaFin.max = maxEndDate.toISOString().split('T')[0];
            
            // Si la fecha fin actual es inválida, limpiarla
            if (fechaFin.value && new Date(fechaFin.value) > maxEndDate) {
                fechaFin.value = '';
            }
        });

        fechaFin.addEventListener('change', function() {
            const startDate = new Date(fechaInicio.value);
            const endDate = new Date(this.value);
            const diffTime = Math.abs(endDate - startDate);
            const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

            if (diffDays > 6) {
                alert('La diferencia entre fechas no puede ser mayor a 6 días');
                this.value = '';
            }
        });
    }
}

function showSection(sectionName) {
    // Ocultar todas las secciones
    document.querySelectorAll('.content-section').forEach(section => {
        section.classList.remove('active');
    });
    
    // Mostrar sección solicitada
    const targetSection = document.getElementById(sectionName + '-section');
    if (targetSection) {
        targetSection.classList.add('active');
    }
    
    // Actualizar título
    const titles = {
        'citas': 'Citas Agendadas',
        'configuracion': 'Configuración de Turnos',
        'servicios': 'Gestión de Servicios',
        'lugares': 'Gestión de Lugares'
    };
    
    const titleElement = document.getElementById('section-title');
    if (titleElement && titles[sectionName]) {
        titleElement.textContent = titles[sectionName];
    }

    // Mostrar/ocultar botón nueva configuración
    const btnNuevaConfig = document.getElementById('btn-nueva-configuracion');
    if (btnNuevaConfig) {
        btnNuevaConfig.style.display = sectionName === 'configuracion' ? 'none' : 'inline-block';
    }
    
    currentSection = sectionName;
}

function handleConfigSubmit(e) {
    e.preventDefault();
    
    const formData = new FormData(e.target);
    const config = {
        fechaInicio: formData.get('fecha-inicio') || document.getElementById('fecha-inicio').value,
        fechaFin: formData.get('fecha-fin') || document.getElementById('fecha-fin').value,
        horaInicio: formData.get('hora-inicio') || document.getElementById('hora-inicio').value,
        horaFin: formData.get('hora-fin') || document.getElementById('hora-fin').value,
        lugares: Array.from(document.getElementById('lugares-config').selectedOptions).map(opt => opt.value),
        servicios: Array.from(document.getElementById('servicios-config').selectedOptions).map(opt => opt.value)
    };

    // Validaciones
    if (!config.fechaInicio || !config.fechaFin || !config.horaInicio || !config.horaFin) {
        alert('Por favor complete todos los campos obligatorios');
        return;
    }

    if (config.lugares.length === 0) {
        alert('Debe seleccionar al menos un lugar');
        return;
    }

    if (config.servicios.length === 0) {
        alert('Debe seleccionar al menos un servicio');
        return;
    }

    // Validar que hora fin sea mayor que hora inicio
    if (config.horaInicio >= config.horaFin) {
        alert('La hora de fin debe ser mayor que la hora de inicio');
        return;
    }

    // Simular guardado exitoso
    alert('Configuración guardada exitosamente');
    
    // Limpiar formulario
    e.target.reset();
    
    // Actualizar lista de configuraciones (simulado)
    agregarConfiguracionALista(config);
}

function agregarConfiguracionALista(config) {
    // Esta función simularía agregar la nueva configuración a la lista
    console.log('Nueva configuración agregada:', config);
}

function filtrarCitasPorFecha() {
    const fechaFiltro = document.getElementById('filtro-fecha').value;
    const filas = document.querySelectorAll('#citas-table tr');
    
    filas.forEach(fila => {
        const fechaCita = fila.cells[3]?.textContent; // Columna de fecha
        if (fechaFiltro && fechaCita) {
            // Convertir fechas para comparar
            const fechaCitaFormatted = fechaCita.split('/').reverse().join('-');
            if (fechaCitaFormatted.includes(fechaFiltro)) {
                fila.style.display = '';
            } else {
                fila.style.display = 'none';
            }
        } else {
            fila.style.display = '';
        }
    });
}

function verDetalles(citaId) {
    // Simular mostrar detalles de la cita
    alert(`Mostrando detalles de la cita ID: ${citaId}`);
}

function cancelarCita(citaId) {
    if (confirm('¿Está seguro que desea cancelar esta cita?')) {
        // Simular cancelación
        alert(`Cita ID ${citaId} cancelada exitosamente`);
        
        // Actualizar tabla (simulado)
        const fila = document.querySelector(`#citas-table tr:nth-child(${citaId})`);
        if (fila) {
            const estadoCell = fila.cells[5];
            estadoCell.innerHTML = '<span class="badge bg-danger">Cancelada</span>';
        }
    }
}

function eliminarConfiguracion(configId) {
    if (confirm('¿Está seguro que desea eliminar esta configuración de turnos?')) {
        alert(`Configuración ${configId} eliminada exitosamente`);
        
        // Simular eliminación del DOM
        const configItem = document.querySelector(`.configuracion-item:nth-child(${configId})`);
        if (configItem) {
            configItem.style.display = 'none';
        }
    }
}

// Funciones para gestión de servicios
function editarServicio(servicioId) {
    alert(`Editando servicio ID: ${servicioId}`);
}

function eliminarServicio(servicioId) {
    if (confirm('¿Está seguro que desea eliminar este servicio?')) {
        alert(`Servicio ${servicioId} eliminado exitosamente`);
    }
}

// Funciones para gestión de lugares
function editarLugar(lugarId) {
    alert(`Editando lugar ID: ${lugarId}`);
}

function eliminarLugar(lugarId) {
    if (confirm('¿Está seguro que desea eliminar este lugar?')) {
        alert(`Lugar ${lugarId} eliminado exitosamente`);
    }
}

// Responsive - Toggle sidebar en móvil
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('open');
}

// Agregar botón de menú en móvil
if (window.innerWidth <= 768) {
    const header = document.querySelector('.content-header');
    const menuButton = document.createElement('button');
    menuButton.innerHTML = '<i class="fas fa-bars"></i>';
    menuButton.className = 'btn btn-outline-secondary btn-sm me-3';
    menuButton.onclick = toggleSidebar;
    
    if (header) {
        header.insertBefore(menuButton, header.firstChild);
    }
}