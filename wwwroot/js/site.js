// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function calculateTotal(unitPrice) {
    var seatsInput = document.getElementById('tripInput');
    var totalElement = document.getElementById('Totali');

    seatsInput.addEventListener('input', function () {
        // Parse the input value to a number; if it's not a valid number, default to 0
        var inputValue = parseFloat(seatsInput.value) || 0;

        var total = inputValue * unitPrice;

        totalElement.textContent = 'Total: ' + total;
    });
}





// Call the function when the DOM is fully loaded
document.addEventListener('DOMContentLoaded', calculateTotal);




function confirmTravel(qty, reservationDate, event) {
    event.preventDefault();

    var inputTrip = document.querySelector('#tripInput');

    // Parse the input value as an integer
    var inputValue = parseInt(inputTrip.value);

    // Check if reservation date is in the past
    if (new Date(reservationDate) < new Date()) {
        Swal.fire({
            icon: 'error',
            title: 'Rezervimi nuk eshte i mundur!',
            text: 'Data e udhëtimit ka kaluar, nuk mund të bëhet rezervim për këtë udhëtim.',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });
        return; // Stop further execution if the reservation date has passed
    }

    if (inputValue > qty) {
        Swal.fire({
            icon: 'error',
            title: 'Nuk ka mjaftueshem vende!',
            text: 'Na vjen keq, por numri i vendeve që ju kërkoni të rezervoni e kalon kapacitetin.',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'OK'
        });

        inputTrip.value = qty;
    } else {
        Swal.fire({
            title: 'A jane detajet ne rregull?',
            text: 'Ju lutem kontrolloni me vemendje te gjitha detajet e biletes',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Po, e konfirmoj!'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    icon: 'success',
                    title: 'Sukses!',
                    text: 'Faleminderit per rezervimin.',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK'
                }).then(() => {
                    var reservationForm = document.getElementById('reservationForm');
                    if (reservationForm) {
                        reservationForm.submit();
                    } else {
                        console.error('Form with ID "reservationForm" not found.');
                    }
                });
            }
        });
    }
}

