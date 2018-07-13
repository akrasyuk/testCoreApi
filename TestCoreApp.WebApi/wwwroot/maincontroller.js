var newRecord = {};
var sorting = 'normal';
var sortBy;
var currentPage = 1;
var pageItemsQuantity = 3; 


function GetResources(filterValue, sortOrder) {
    var urlString = '/api/v-1.0/Resources';
    var separator = '?';

    if (filterValue) {
        urlString += separator + 'filter=' + filterValue;
        separator = '&';
    }
    if (sortOrder) {
        urlString += separator + 'sort=' + sortOrder;
        separator = '&';
    }
   
    $.ajax({
        url: urlString,
        type: 'GET',
        contentType: "application/json",
        success: function (resources) {
            var rows = "";
            $.each(resources, function (index, resource) {

                rows += row(resource);
            });
            $("table tbody").empty();
            $("table tbody").append(rows);
        }
    });
}

function GetResource(id) {
    $.ajax({
        url: '/api/v-1.0/Resources/' + id,
        type: 'GET',
        contentType: "application/json",
        success: function (resource) {
            var form = document.forms["resourceForm"];
            form.elements["id"].value = resource.id;
            form.elements["name"].value = resource.name;

            form.elements["createdBy"].value = resource.createdBy;
            form.elements["createdOnUtc"].value = resource.createdOnUtc;
            form.elements["updatedOnUtc"].value = resource.updatedOnUtc;
            form.elements["updatedBy"].value = resource.updatedBy;

            form.elements["customProperty1"].value = resource.customProperty1;
            form.elements["customProperty2"].value = resource.customProperty2;
            form.elements["customProperty3"].value = resource.customProperty3;
            form.elements["customProperty4"].value = resource.customProperty4;
            form.elements["customProperty5"].value = resource.customProperty5;
            form.elements["customProperty6"].value = resource.customProperty6;
            form.elements["customProperty7"].value = resource.customProperty7;
        }
    });
}

function CreateRecord(newRecord) {
    $.ajax({
        url: "/api/v-1.0/Resources/",
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            name: newRecord.Name,
            createdOnUtc: newRecord.CreatedOnUtc,
            createdBy: 1,
            updatedOnUtc: newRecord.UpdatedOnUtc,
            updatedBy: 1,
            customProperty1: newRecord.CustomProperty1,
            customProperty2: newRecord.CustomProperty2,
            customProperty3: newRecord.CustomProperty3,
            customProperty4: newRecord.CustomProperty4,
            customProperty5: newRecord.CustomProperty5,
            customProperty6: newRecord.CustomProperty6,
            customProperty7: newRecord.CustomProperty7
        }),
        success: function (newrecord) {
            //reset();
            //$("table tbody").append(row(newrecord));
            GetPages(pageItemsQuantity, currentPage, $("#filter").val(), sorting);
            return true;
        },

        error: function (errormessage) {
            alert(errormessage.responseText);
            errormessage = null;
            return false;
        }

    });
}

function PatchResource(id) {
    $.ajax({
        url: "api/v-1.0/Resources/" + id,
        contentType: "application/json",
        method: "PATCH",
        data: JSON.stringify(
            [{ PropertyName: "Name", PropertyValue: "Alex" },
            { PropertyName: "CustomProperty1", PropertyValue: 2 },
            { PropertyName: "CustomProperty2", PropertyValue: "777777" },
            { PropertyName: "CreatedBy", PropertyValue: "3" }
            ]
        ),
        success: function (message) {
            alert("Resource patched!");
        }
    });
}

function GetPages(itemsPerPage, page, filterValue, sortOrder, orderBy) {
    var urlString = '/api/v-1.0/ResourcesPaginated';
    var separator = '?';

    if (itemsPerPage) {
        urlString += separator + 'itemsperpage=' + itemsPerPage;
        separator = '&';
    }
    if (page) {
        urlString += separator + 'page=' + page;
        separator = '&';
    }    
    if (filterValue) {
        urlString += separator + 'filter=' + filterValue;
        separator = '&';
    }
    if (sortOrder) {
        urlString += separator + 'sort=' + sortOrder;
    }
    if (orderBy) {
        urlString += separator + 'orderby=' + orderBy;
    }
    $.ajax({
        url: urlString,
        method: "GET",
        contentType: "application/json",
        success: function (pages) {
            var rows = "";
            $.each(pages.data, function (index, resource) {
                rows += row(resource);
            });

            if (pages.index === 1) { $("#previousbutton").prop("disabled", true); }
            else { $("#previousbutton").prop("disabled", false); }
            if (pages.index === pages.totalPages) { $("#nextbutton").prop("disabled", true); }
            else { $("#nextbutton").prop("disabled", false); }

            $("table tbody").empty();
            $("table tbody").append(rows);
        }
    });
}

function DeleteResource(id) {
    $.ajax({
        url: "api/v-1.0/resources/" + id,
        contentType: "application/json",
        method: "DELETE",
        success: function (resource) {

            alert("Resource deleted!");
        }
    });
}

var row = function (resource) {
    return "<tr data-rowid='" + resource.id + "'><td>" + resource.id + "</td>" +
        "<td>" + resource.name + "</td> " + "<td><a href='#' class='detailsLink' data-id='" + resource.id + "'>Details</a> | <a href='#' class='editLink' data-id='" + resource.id + "'>Change</a> | " +
        "<a href class='removeLink' data-id='" + resource.id + "'>Delete</a></td></tr>";
};

$("body").on("click", ".removeLink", function () {
    var id = $(this).data("id");
    DeleteResource(id);
    GetPages(pageItemsQuantity, currentPage, $("#filter").val(), sorting);

});

$("body").on("click", ".editLink", function () {
    var id = $(this).data("id");
    PatchResource(id);
    GetPages(pageItemsQuantity, currentPage, $("#filter").val(), sorting, sortBy);
});

$("body").on("click", ".detailsLink", function () {
    $('#detailsModal').modal('show');
    var id = $(this).data("id");
    GetResource(id);
});

//$("body").on("click", "#refresh", function () {
//    GetPages(pageItemsQuantity, 1);  
//})

$("body").on("click", "#idsortlink", function () {
    if (sorting === "Normal") {
        sorting = 'Invert';
        sortBy = 'Id';
    }
    else {
        sorting = 'Normal';
        sortBy = 'Id';
    }
    GetPages(pageItemsQuantity, currentPage, $("#filter").val(), sorting, sortBy);
});

$("body").on("click", "#sortlink", function () {
    if (sorting === "Normal") {
        sorting = 'Invert';
        sortBy = 'Name';
    }
    else {
        sorting = 'Normal';
        sortBy = 'Name';
    }
    GetPages(pageItemsQuantity, currentPage, $("#filter").val(), sorting, sortBy);
});

$("body").on("click", "#filterbutton", function () {

    GetPages(pageItemsQuantity, 1, $("#filter").val(), sorting, sortBy);
});
$("body").on("click", "#addrecordbutton", function () {
    newRecord.Name = $("#createname").val();
    newRecord.CreatedBy = 1;
    newRecord.UpdatedOnUtc = new Date();
    newRecord.UpdatedBy = 1;
    newRecord.CreatedOnUtc = new Date();
    newRecord.CustomProperty1 = $("#createcustomproperty1").val();
    newRecord.CustomProperty2 = $("#createcustomproperty2").val();
    newRecord.CustomProperty3 = $("#createcustomproperty3").val();
    newRecord.CustomProperty4 = $("#createcustomproperty4").val();
    newRecord.CustomProperty5 = $("#createcustomproperty5").val();
    newRecord.CustomProperty6 = $("#createcustomproperty6").val();
    newRecord.CustomProperty7 = $("#createcustomproperty7").val();
    if (CreateRecord(newRecord)) { $('#createrecordModal').modal('hide'); }

});

$("body").on("click", "#nextbutton", function () {
    var currentFilter = $("#filter").val();
    currentPage = currentPage + 1;
    GetPages(pageItemsQuantity, currentPage, currentFilter, sorting, sortBy);
    $("#filter").val(currentFilter);
});
$("body").on("click", "#previousbutton", function () {
    var currentFilter = $("#filter").val();
    currentPage = currentPage - 1;
    GetPages(pageItemsQuantity, currentPage, currentFilter, sorting, sortBy);
    $("#filter").val(currentFilter);
});

GetPages(pageItemsQuantity, 1); 