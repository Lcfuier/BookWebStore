$('#book-upsert-form').submit(function (event) {
    // Check if at least one checkbox is selected
    const isChecked = $('input[name="BookDto.CategoryIds"]:checked').length > 0;

    // If no checkbox is selected, display an error message and prevent form submission
    if (!isChecked) {
        event.preventDefault();
        $('#category-error').text("Vui lòng chọn ít nhất 1 thể loại.");
    }
});