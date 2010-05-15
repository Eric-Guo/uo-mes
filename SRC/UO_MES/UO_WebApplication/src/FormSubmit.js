var formSubmitCount = 0;
function FormCheck_OnSubmit() {
    if (formSubmitCount == 0) {
        formSubmitCount++;
    }
    else {
        /* Disable alert & only return false will make user feel better
        alert('Transaction is processing, Please Wait....');
        */
        return false;
    }
    return true;
}