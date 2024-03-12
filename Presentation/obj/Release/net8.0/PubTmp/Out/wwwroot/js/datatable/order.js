let dataTable;
$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
        var status = urlParams.get('status');
        loadDataTable(status);
})
function loadDataTable(status) {
    dataTable = $('#OrderData').DataTable({
        "ajax": {
            type : "GET",
            url: `/librarian/order/getall?status=${status}`
        },
        columns: [
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "paymentStatus", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "totalAmount", "width": "15%" },
            {
                "data": "orderId",
                "render": (data)=> {
                    return `
                        <div class="w-100 pt-2 btn btn-info" role="group">
                            <a href="/librarian/Order/Details/${data}" class="btn btn-dark mx-2" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i> Details
                            </a>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ]
    });
}

