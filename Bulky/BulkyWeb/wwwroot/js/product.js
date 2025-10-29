var dataTable;

$(document).ready(function () {
    initializeFilters();
    loadDataTable();
});

function initializeFilters() {
    $('#applyFilters').on('click', function () {
        dataTable.ajax.reload();
    });

    $('#resetFilters').on('click', function () {
        $('#filterForm').find('input[type="text"], input[type="number"]').val('');
        $('#filterForm').find('select').val('');
        dataTable.ajax.reload();
    });

    $('#filterForm').on('keypress', 'input', function (event) {
        if (event.which === 13) {
            event.preventDefault();
            dataTable.ajax.reload();
        }
    });
}

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url:'/admin/product/getall',
            data: function (d) {
                d.categoryId = $('#SelectedCategoryId').val();
                d.searchTerm = $('#SearchTerm').val();
                d.minPrice = $('#MinPrice').val();
                d.maxPrice = $('#MaxPrice').val();
            }
        },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            {
                data: 'price',
                "width": "10%",
                "render": function (data) {
                    return formatCurrency(data);
                }
            },
            {
                data: 'price50',
                "width": "10%",
                "render": function (data) {
                    return formatCurrency(data);
                }
            },
            {
                data: 'price100',
                "width": "10%",
                "render": function (data) {
                    return formatCurrency(data);
                }
            },
            { data: 'author', "width": "15%" },
            { data: 'categoryName', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                     <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function formatCurrency(value) {
    if (value === null || value === undefined) {
        return '—';
    }

    var amount = Number(value);
    if (isNaN(amount)) {
        return '—';
    }

    return amount.toLocaleString(undefined, { style: 'currency', currency: 'USD' });
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
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message || 'Unable to delete the product.');
                    }
                },
                error: function () {
                    toastr.error('An unexpected error occurred while deleting the product.');
                }
            })
        }
    })
}