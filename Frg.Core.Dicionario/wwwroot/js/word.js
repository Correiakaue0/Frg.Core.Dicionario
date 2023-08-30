const store = new DevExpress.data.CustomStore({
    key: 'name',
    remove: function (key) {
        $.ajax({
            url: '/api/WordApi/' + key,
            type: 'DELETE',
            success: function (data) {
                message("Palavra deletada com sucesso!", "success")
            },
            error: function (data) {
                message("Palavra nao deletada com sucesso!", "error")
            }
        });
    },
    update: function (key, value) {
        var obj = {
            Name: key,
            Portuguese: value.portuguese,
            English: value.english,
            Spanish: value.spanish
        }
        $.ajax({
            url: '/api/WordApi/',
            type: 'PUT',
            headers: { "content-type": "application/json" },
            data: JSON.stringify(obj),
            success: function (data) {
                message("Palavra atualizada com sucesso!", "success")
            },
            error: function (data) {
                message("Palavra nao atualizada com sucesso!", "error")
            }
        });
    },
    insert: function (values) {
        $.ajax({
            url: '/api/WordApi',
            type: 'POST',
            headers: { "content-type": "application/json" },
            data: JSON.stringify(values),
            success: function (data) {
                message("Palavra inserida com sucesso!", "success")
            },
            error: function (data) {
                message(data.responseText, "error")
            }
        });
    },
    load(loadOptions) {
        const deferred = $.Deferred();
        $.ajax({
            url: '/api/WordApi',
            dataType: 'json',
            success(result) {
                deferred.resolve(result, {
                    totalCount: 3,
                    summary: result.summary,
                    groupCount: result.groupCount,
                });
            },
            error() {
                deferred.reject('Não foi encontrado nenhuma palavra');
            },
            timeout: 10000,
        });

        return deferred.promise();
    },
});

$('#grid').dxDataGrid({
    dataSource: store,
    keyExpr: 'name',
    editing: {
        mode: 'row',
        allowUpdating: true,
        allowAdding: true,
        allowDeleting: true,
        useIcons: true,
    },
    columns: [{
        dataField: 'name',
    }, {
        dataField: 'portuguese',
    },
    {
        dataField: 'english',
    },
    {
        dataField: 'spanish',
    }],
    showBorders: true,
    paging: {
        pageSize: 10,
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [10, 25, 50, 100],
    },
    searchPanel: {
        visible: true,
        highlightCaseSensitive: false,
    },
    allowColumnReordering: true,
    rowAlternationEnabled: true,
    showBorders: true
});

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