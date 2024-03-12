let dataTable;

$(document).ready(() => {
    dataTable = $('#datatable-user').DataTable({
        "ajax": {
            "type": "GET",
            "url": "/admin/user/getAllUsers"
        },
        "columns": [
            { "data": "lastName", "width": "15%" },
            { "data": "firstName", "width": "15%" },
            { "data": "email", "width": "20%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    "id": "id",
                    "lockoutEnd": "lockoutEnd"
                },
                "render": (data) => {
                    const currentDate = new Date();
                    let dateLockoutEnd;

                    if (data.lockoutEnd != null) {
                        // console.log("is not null");
                        dateLockoutEnd = new Date(data.lockoutEnd);
                    } else {
                        // console.log("is null");
                        dateLockoutEnd = new Date("1000-01-01");
                    }

                    // const currentDateString = currentDate.toDateString();
                    // const dateLockoutEndString = dateLockoutEnd.toDateString();
                    // console.log(`${dateLockoutEndString} and ${currentDateString}`);

                    if (dateLockoutEnd > currentDate) {
                        // console.log(`${dateLockoutEndString} is greater than ${currentDateString}`);
                        return `
                            <div class="text-center">
                                <a class="btn btn-success text-white"
                                    onclick=LockUnlock("/admin/user/lockUnlockUser/${data.id}")>
                                    <span class="fas fa-lock-open"></span>&nbsp;Mở khóa
                                </a>
                            </div>
                            `;
                    } else {
                        // console.log(`${dateLockoutEndString} is smaller than ${currentDateString}`);
                        return `
                            <div class="text-center">
                                <a class="btn btn-danger text-white"
                                    onclick=LockUnlock("/admin/user/lockUnlockUser/${data.id}")>
                                    <span class="fas fa-lock"></span>&nbsp;Khóa
                                </a>
                            </div>
                            `;
                    }
                },
                "width": "20%"
            }
        ]
    });
});

function LockUnlock(url) {
    Swal.fire({
        title: "Bạn có chắc muốn khóa/mở khóa cho tài khoảng này?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Khóa/Mở khóa",
        cancelButtonText: "Hủy"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: url,
                success: (data) => {
                    if (data.success) { // if success is then then get the data
                        Swal.fire({
                            title: "Thành công!",
                            text: data.message,
                            icon: "success"
                        });
                        dataTable.ajax.reload();
                    } else {
                        Swal.fire({
                            title: "Lỗi!!!",
                            text: data.message,
                            icon: "error"
                        });
                    }
                }
            })
        }
    });
}