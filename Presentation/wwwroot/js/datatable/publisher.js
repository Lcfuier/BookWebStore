let dataTable;

$(document).ready(() => {
    dataTable = $('#datatable-publisher').DataTable({
        "ajax": {
            "type": "GET",
            "url": '/librarian/publisher/getAllPublishers'
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "publisherId",
                "render": (data) => {
                    return `
                        <div class="text-center">
                            <a class="btn btn-success text-white"
                                href="/librarian/publisher/upsert/${data}">
                                <span class="fas fa-edit"></span>&nbsp;Sửa
                            </a>
                            <a class="btn btn-danger text-white"
                                onclick=Delete("/librarian/publisher/deletePublisher/${data}")>
                                <span class="fas fa-trash-alt"></span>&nbsp;Xóa
                            </a>
                        </div>
                        `;
                },
                "width": "40%"
            }
        ]
    });
});