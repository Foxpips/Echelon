var Employee = (function () {
    function Employee() {
    }
    Employee.prototype.setFirstName = function (value) {
        this.firstName = value;
    };
    Employee.prototype.setLastName = function (value) {
        this.lastName = value;
    };
    Employee.prototype.getFullName = function (lastNameFirst) {
        if (lastNameFirst === void 0) { lastNameFirst = false; }
        if (lastNameFirst) {
            return this.lastName + ", " + this.firstName;
        }
        return this.firstName + ", " + this.lastName;
    };
    return Employee;
}());
