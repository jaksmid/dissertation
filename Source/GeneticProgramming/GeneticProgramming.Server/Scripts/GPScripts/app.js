var app = angular.module('GpModule', ['ui.bootstrap']);

app.factory('newService', function () {
    var shinyNewServiceInstance = {};
    shinyNewServiceInstance.subscribers = new Array();
    // Declare a proxy to reference the hub.
    var chat = $.connection.gpProgressHub;
    // Create a function that the hub can call to broadcast messages.
    chat.client.updateProgress = function(validationProgress) {
        for (var i = 0; i < shinyNewServiceInstance.subscribers.length; i++) {
            shinyNewServiceInstance.subscribers[i](validationProgress);
        }
    };

    chat.client.broadcastMessage = function (name, message) {
        // Html encode display name and message.
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();
        // Add the message to the page.
        $('#discussion').append('<li><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
    };
    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            chat.server.sendMessage($('#displayname').val(), $('#message').val());
            // Clear text box and reset focus for next comment.
            $('#message').val('').focus();
        });
    });
    shinyNewServiceInstance.subscribe = function (methodToNotify) {
        //var x = shinyNewServiceInstance.ahoj.hj;
        shinyNewServiceInstance.subscribers.push(methodToNotify);
    };
    return shinyNewServiceInstance;
});

var progressCtrl = function($scope,newService) {
    this.toValidate = 100;
    this.toEvaluate = 100;
    this.evaluatedPercentage = 0;
    this.validatedPercentage = 0;
    this.validated = 0;
    this.evaluated = 0;
    this.generationNumber = 0;
    this.experimentName = "";
    var currentObject = this;
    this.updateProgress = function (genProgressMessage) {
        if (genProgressMessage.ToValidate === 0) {
            currentObject.validatedPercentage = 100;
        } else {
            currentObject.validatedPercentage = (genProgressMessage.Validated / genProgressMessage.ToValidate) * 100;
        }
        currentObject.validated = genProgressMessage.Validated;
        currentObject.evaluated = genProgressMessage.Evaluated;
        if (genProgressMessage.ToEvaluate === 0) {
            currentObject.evaluatedPercentage = 100;
        } else {
            currentObject.evaluatedPercentage = (genProgressMessage.Evaluated / genProgressMessage.ToEvaluate) * 100;
        }
        
        currentObject.toValidate = genProgressMessage.ToValidate;
        currentObject.toEvaluate = genProgressMessage.ToEvaluate;
        currentObject.generationNumber = genProgressMessage.GenerationNumber;
        currentObject.experimentName = genProgressMessage.ExperimentName;
        //TODO: improve speed
        $scope.$apply();
    };
    newService.subscribe(this.updateProgress);
};

app.controller('ProgressDemoCtrl', ['$scope','newService', progressCtrl]);