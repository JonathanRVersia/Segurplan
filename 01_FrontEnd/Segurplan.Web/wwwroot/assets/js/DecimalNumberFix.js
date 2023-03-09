if (typeof cultureInfo != 'undefined') {
    if (cultureInfo == "es-ES") {
        $.validator.methods.number = function (value, element) {
            return this.optional(element) || /^(?:\d+|\d{1,3}(?:[\s\,]\d{3})+)(?:[\,]\d+)?$/.test(value);
        }

    }
    else {
        $.validator.methods.number = function (value, element) {
            return this.optional(element) || /^(?:\d+|\d{1,3}(?:[\s\.]\d{3})+)(?:[\.]\d+)?$/.test(value);
        }
    }
}