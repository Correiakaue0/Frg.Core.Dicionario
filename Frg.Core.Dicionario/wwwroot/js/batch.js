function traduzir() {
    var stringPure = document.getElementById("formControlTradutions").value;

    var receive = {
        Receive: stringPure
    }
    $.ajax({
        url: '/api/WordApi/Batch',
        type: 'POST',
        headers: { "content-type": "application/json" },
        data: JSON.stringify(receive),
        success: function (data) {
            message("Palavra atualizada com sucesso!", "success")
        },
        error: function (data) {
            message("Palavra nao atualizada com sucesso!", "error")
        }
    });
}


function message(message, type) {
    let direction = 'down-stack';
    let position = 'top right';
    DevExpress.ui.notify({
        message: message,
        height: 45,
        width: 300,
        minWidth: 150,
        type: type,
        displayTime: 3500,
        animation: {
            show: {
                type: 'fade', duration: 400, from: 0, to: 1,
            },
            hide: { type: 'fade', duration: 40, to: 0 },
        },
    },
        {
            position,
            direction,
        });
}