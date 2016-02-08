var app = angular.module('TasksApp', []);
app.controller('TasksCtrl', function ($scope, $http) {
    $scope.EditedTask = {};
    $scope.UserName = {};
    $scope.UserNames = [""];
    $scope.CreatedTasks = [""];
    $scope.ResponsableTasks = [""];

    $scope.GetUserName = function () {
        $http.get("/api/Other/GetUserName")
        .then(function (response) {
            $scope.UserName = response.data;
        })
        .catch(console.log.bind(console));
    };

    $scope.GetUserNames = function () {
        $http.get("/api/Other/GetUserNames")
        .then(function (response) {
            $scope.UserNames = response.data;
        })
        .catch(console.log.bind(console));
    };

    $scope.GetCreatedTasks = function () {
        $http.get("/api/CreatedTasks/Get")
        .then(function (response) {
            $scope.CreatedTasks = response.data;
        })
        .catch(console.log.bind(console));
    };

    $scope.GetResponsableTasks = function () {
        $http.get("/api/ResponsableTask/Get")
        .then(function (response) {
            $scope.ResponsableTasks = response.data;
        })
        .catch(console.log.bind(console));
    };

    

    //remove task by REST post action
    $scope.RemoveCreatedTask = function (task, index) {
        $http.post("/api/CreatedTasks/Delete?TaskID=" + task.TaskID)
        .then(function (response) {
            $scope.CreatedTasks.splice(index, 1);
            $scope.ResponsableTasks.splice(index, 1);
            toastr.info('Remove succesful');
        })
        .catch(console.log.bind(console));
    };


    //save to temp value choosen task
    $scope.ChooseTask = function (task) {
        $scope.EditedTask = task;
        $scope.CopyProperties(task, $scope.TempTask);
    };

    //edit task by REST post action
    $scope.EditTask = function (task) {
        $http.post('/Tasks/Edit', { taskViewModel: task })
            .success(function () {
                $scope.CopyProperties(task, $scope.EditedTask);
                toastr.info('Edit succesful');
            });
    };

    //copy properties (map)
    $scope.CopyProperties = function (obj, copy) {
        for (var propety in obj)
            copy[propety] = copy[propety];
    };

    //change status
    $scope.ChangeTaskStatus = function (sourceTask, newStatus) {
        sourceTask.Status = newStatus;
        $http.post('/Tasks/Edit', { taskViewModel: sourceTask })
            .success(function () {
                $scope.CopyProperties(sourceTask, $scope.EditedTask);
                toastr.info('Task (' + sourceTask.Name + ') status change to "' + newStatus + '"');
            });
    };
});
