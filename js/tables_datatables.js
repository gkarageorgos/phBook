var globalLang = {
    buttons: {
        selectAll: "Επιλογή όλων",
        selectNone: "Επιλογή κανενός"
    },
    lengthMenu: "Εμφάνισε _MENU_ αποτελέσματα",
    search: "Αναζήτηση",
    zeroRecords: "Δεν βρέθηκαν Αποτελέσματα...",
    paginate: {
        "first": "Πρώτη",
        "last": "Τελευταία",
        "next": "Επόμενη",
        "previous": "Προηγούμενη"
    },
    info: "Εμφανίζονται _START_ εώς _END_ από _TOTAL_ αποτελέσματα",
    infoFiltered: "(φίλτρο σε  _MAX_ συνολικά αποτελέσματα)",
    infoEmpty: "Εμφάνιση 0 εώς 0 από 0 αποτελέσματα",
    select: {
        rows: "%d επιλεγμένες γραμμές",
        cells: "%d επιλεγμένα κελιά"
    },
    decimal: ",",
    thousands: "."
}

var globalDom = '<"html5buttons"B>lTfgitp';
globalDom = '<"html5buttons"B>lTfgtpi';


$(document).ready(function () {

    if ($(".datatables").length>0) {
        $(".datatables").DataTable({
            "order": [],
            dom: globalDom,
            buttons: [],
            responsive: true,
            language: globalLang,
            initComplete: function () {
                $(this).show();
            }
        });
    }
})