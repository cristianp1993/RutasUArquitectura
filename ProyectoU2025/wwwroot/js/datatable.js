    // Call the dataTables jQuery plugin
    $(document).ready(function() {
        $('#dataTable').DataTable({
        paging: false,      // Desactiva la paginación
        searching: false, // Desactiva la búsqueda
        info: false,        // (Opcional) Oculta la información de registros
    });
    });