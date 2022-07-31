window.ShowToastr = (type, message) => {
    if(type === "success"){
        toastr.success(message, "Operation Successful", {timeOut: 5000});
    }
    if(type === "error"){
        toastr.error(message, "Operation Failed", {timeOut: 5000});
    }
}

window.ShowSweet = (type, title, text) => {

    
    Swal.fire({
        icon: type,
        title: title,
        text: text,
        footer: '<a href="">Why do I have this issue?</a>'
    });

}

function ShowDeleteConfirmationModal(){
    $('#deleteConfirmationModal').modal('show');
}

function HideDeleteConfirmationModal(){
    $('#deleteConfirmationModal').modal('hide');
}
