function Delete(url) {
    Swal.fire({
        title: "Bạn có chắc muốn xóa?",
        text: "Khi xóa sẽ xóa vĩnh viễn và sẽ không thể hồi phục.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Xóa",
        cancelButtonText: "Hủy"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: (data) => {
                    if (data.success) { // if success is then then get the data
                        Swal.fire({
                            title: "Đã xóa!",
                            text: data.message,
                            icon: "success"
                        }).then(() => {
                            window.location.reload();
                        });
                    } else {
                        Swal.fire({
                            title: "Lỗi!!!",
                            text: data.message,
                            icon: "error"
                        }).then(() => {
                            window.location.reload();
                        });
                    }
                }
            })
        }
    });
}