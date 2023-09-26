var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: '/admin/product/getall'
        },
        columns: [
            { data: 'title', className: 'align-middle', width: '25%' },
            { data: 'isbn', className: 'align-middle', width: '15%' },
            { data: 'listPrice', className: 'align-middle', width: '10%' },
            { data: 'author', className: 'align-middle', width: '15%' },
            { data: 'category.name', className: 'align-middle', width: '10%' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="text-center align-middle btn-container">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick="Delete('/admin/product/delete/${data}')" class="btn btn-danger">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>
                    `;
                },
                width: '25%'
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then(function (result) {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
