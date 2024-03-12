$(() => {
    $('#publisher-name').autocomplete({
        source: (request, response) => {
            $.ajax({
                url: "/librarian/publisher/searchPublishers",
                dataType: "json",
                data: {
                    "term": request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1,
        select: (event, ui) => {
            $('#publisher-id').val(ui.item.id);
        }
    });
});