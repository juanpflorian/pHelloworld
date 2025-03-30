document.addEventListener("DOMContentLoaded", function () {
    console.log("✅ DOM completamente cargado");

    const reservaForm = document.getElementById("reservaForm");
    const modalReserva = document.getElementById("reservaModal");

    if (!modalReserva) {
        console.error("❌ ERROR: No se encontró el modal de reserva.");
        return;
    }

    document.querySelectorAll(".reservar-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            console.log("🔹 Botón de reserva clickeado");

            const idPlan = this.getAttribute("data-idplan");
            const idGuia = this.getAttribute("data-idguia");

            console.log("📌 ID del plan:", idPlan, " | ID del guía:", idGuia);

            const idPlanInput = document.getElementById("idPlan");
            const idGuiaInput = document.getElementById("idGuia");

            if (!idPlanInput || !idGuiaInput) {
                console.error("❌ ERROR: No se encontraron los campos ocultos de ID.");
                return;
            }

            idPlanInput.value = idPlan;
            idGuiaInput.value = idGuia;

            const modal = new bootstrap.Modal(modalReserva);
            modal.show();
        });
    });

    if (reservaForm) {
        reservaForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const idPlan = document.getElementById("idPlan")?.value;
            const idGuia = document.getElementById("idGuia")?.value;
            const fechaProgramada = document.getElementById("fechaProgramada")?.value;

            if (!idPlan || !idGuia || !fechaProgramada) {
                alert("❌ Error: Todos los campos son obligatorios.");
                return;
            }

            console.log("✅ Reserva enviada:", { idPlan, idGuia, fechaProgramada });

            alert("✅ Reserva realizada con éxito");
        });
    } else {
        console.warn("⚠️ Advertencia: No se encontró el formulario de reserva.");
    }
});
