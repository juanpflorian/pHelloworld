document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM completamente cargado");

    agregarEventosReserva();
    manejarFormularioReserva();
});

// Agrega eventos a los botones de reserva
function agregarEventosReserva() {
    const reservarBotones = document.querySelectorAll('.reservar-btn');

    if (reservarBotones.length === 0) {
        console.warn("No se encontraron botones de reserva.");
        return;
    }

    reservarBotones.forEach(boton => {
        boton.addEventListener('click', function () {
            const idPlan = this.getAttribute('data-idplan');
            const idGuia = this.getAttribute('data-idguia');
            const nombrePlan = this.closest('tr').querySelector('td:nth-child(2)').innerText;

            if (!idPlan || !idGuia) {
                console.error("Error: Datos de la reserva no válidos.");
                return;
            }

            abrirModalReserva(idPlan, nombrePlan, idGuia);
        });
    });
}

// Maneja el envío del formulario de reserva
function manejarFormularioReserva() {
    const reservaForm = document.getElementById('reservaForm');

    if (!reservaForm) {
        console.error("Error: Formulario de reserva no encontrado.");
        return;
    }

    reservaForm.addEventListener('submit', function (event) {
        event.preventDefault();

        const idPlan = document.getElementById('idPlan')?.value;
        const fechaProgramada = document.getElementById('fechaProgramada')?.value;
        const idGuia = document.getElementById('idGuia')?.value;

        if (!idPlan || !fechaProgramada || !idGuia) {
            mostrarAlerta("Por favor, completa todos los campos.", "danger");
            return;
        }

        // Validar fecha actual vs fecha seleccionada
        const hoy = new Date();
        hoy.setHours(0, 0, 0, 0);

        const fechaSeleccionada = new Date(fechaProgramada);
        fechaSeleccionada.setHours(0, 0, 0, 0);

        if (fechaSeleccionada < hoy) {
            mostrarAlerta("No puedes reservar una fecha anterior a la de hoy.", "danger");
            return;
        }

        const datosReserva = {
            idPlan: parseInt(idPlan),
            fechaProgramada: fechaProgramada,
            idGuia: parseInt(idGuia)
        };

        console.log("Enviando datos de reserva:", datosReserva);
        realizarReserva(datosReserva);
    });
}

// Enviar la solicitud de reserva al servidor
function realizarReserva(datosReserva) {
    fetch('/Reservas/Reservar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(datosReserva)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => {
                    throw new Error(err.message || "Error en la reserva.");
                });
            }
            return response.json();
        })
        .then(data => {
            console.log("Respuesta del servidor:", data);
            if (data.success) {
                mostrarAlerta("Reserva creada exitosamente.", "success");
                setTimeout(() => location.reload(), 1500);
            } else {
                mostrarAlerta(data.message || "Error en la reserva. Inténtalo nuevamente.", "danger");
            }
        })
        .catch(error => {
            console.error("Error en la solicitud:", error);
            mostrarAlerta(error.message || "Ocurrió un error inesperado. Inténtalo de nuevo.", "danger");
        });
}

// Abre el modal con los datos del plan
function abrirModalReserva(idPlan, nombrePlan, idGuia) {
    const inputIdPlan = document.getElementById('idPlan');
    const inputIdGuia = document.getElementById('idGuia');
    const modalLabel = document.getElementById('reservaModalLabel');
    const modalElement = document.getElementById('reservaModal');

    if (!inputIdPlan || !inputIdGuia || !modalLabel || !modalElement) {
        console.error("Error: Elementos del modal no encontrados.");
        return;
    }

    inputIdPlan.value = idPlan;
    inputIdGuia.value = idGuia;
    modalLabel.innerText = "Reservar: " + nombrePlan;

    const modal = new bootstrap.Modal(modalElement, { keyboard: false });
    modal.show();
}

// Muestra una alerta en pantalla
function mostrarAlerta(mensaje, tipo) {
    const alertMessage = document.getElementById('alertMessage');
    if (!alertMessage) {
        console.error("Error: Elemento de alerta no encontrado.");
        return;
    }

    alertMessage.innerText = mensaje;
    alertMessage.className = `alert alert-${tipo}`;
    alertMessage.classList.remove('d-none');

    setTimeout(() => {
        alertMessage.classList.add('d-none');
    }, 3000);
}
