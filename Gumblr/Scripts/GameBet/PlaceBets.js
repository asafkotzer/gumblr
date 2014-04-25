var PlaceBets = function (model) {
    
    this.model = model;

    this.init = function()
    {
        // apply KO bindings
        ko.applyBindings(new ViewModel(this.model));

        // make the arrow visible on hover
        // TODO: consider using this instead http://jsfiddle.net/tw16/JfK6N/
        $(".Logo").hover(
            function (event) {
                $(this).siblings(".SelectionMarker").addClass("Visible", 500, "easeOutBounce");
            },
            function (event) {
                $(this).siblings(".SelectionMarker").removeClass("Visible", 500, "easeOutBounce");
            });
    }
};