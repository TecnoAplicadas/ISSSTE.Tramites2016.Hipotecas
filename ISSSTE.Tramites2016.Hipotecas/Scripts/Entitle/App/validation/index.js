$("#form").submit(function () {
    debugger;
    if ($("#form").valid())
        $.ajax({
            url: '/Entitle/SaveIndex/',
            
            processData: false,
            contentType: false,
            type: 'POST',
            success: function () {
                alert("qqq");
            }
        
        });

    return true;
});

function runSearch() {
    alert("");
}