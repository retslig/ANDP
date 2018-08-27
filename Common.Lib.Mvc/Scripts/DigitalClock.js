
var CommonTimeObject = function (hour, minute, second, useMilitaryTime) {
    // private members unless used in the return below.
    if (useMilitaryTime) {
        this.hour = hour;
    } else {
        if (hour > 12) {
            this.hour = hour - 12;
        }
    }
    
    this.minute = minute;
    this.second = second;
    this.useMilitaryTime = useMilitaryTime;
    this.isPm = this.hour > 11 && this.hour < 24;
};

CommonTimeObject.prototype = function() {
    // private members unless used in the return below.
    var drawDigitalClock = function(imagePath) {
            if (this.useMilitaryTime) {
                $("#DigitalClockAmPm").attr("src", imagePath + "Blank.gif");
            } else {
                $("#DigitalClockAmPm").attr("src", imagePath + (this.ampm == 'pm' ? 'pm' : 'am') + ".gif");
            }

            var minute = (this.minute < 10 ? "0" + this.minute : this.minute).toString();
            var hour = (this.hour < 10 ? "0" + this.hour : this.hour).toString();

            $("#DigitalClockHour1").attr("src", imagePath + hour.substring(0, 1) + ".gif");
            $("#DigitalClockHour2").attr("src", imagePath + hour.substring(1, 2) + ".gif");
            $("#DigitalClockMinute1").attr("src", imagePath + minute.substring(0, 1) + ".gif");
            $("#DigitalClockMinute2").attr("src", imagePath + minute.substring(1, 2) + ".gif");

            this.minute++;

            if (this.minute >= 60) {
                this.minute = this.minute - 60;
                this.hour++;
            }

            if (this.hour > 24) {
                this.hour = this.hour - 24;
                this.isPm = false;
            }

            if (this.hour > 11 && this.hour < 24) {
                this.isPm = true;
            }

            if (this.hour > 12 && !this.useMilitaryTime) {
                this.hour = this.hour - 12;
            }
        },
        retrieve24HourTimeString = function() {
            var minute = (this.minute < 10 ? "0" + this.minute : this.minute).toString();
            var hour = (this.ampm == 'pm' ? this.hour + 12 : (this.hour < 10 ? "0" + this.hour : this.hour)).toString();
            return hour + ':' + minute;
        },
        retrieve12HourTimeString = function() {
            var minute = (this.minute < 10 ? "0" + this.minute : this.minute).toString();
            var hour = (this.ampm == 'pm' ? this.hour + 12 : (this.hour < 10 ? "0" + this.hour : this.hour)).toString();
            return hour + ':' + minute;
        },
        validateMilitaryTimeOfDay = function (timeOfDay) {
        //Valid formats are HH:MM or H:MM or HHMM.
        var patt1 = new RegExp("^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
        var patt2 = new RegExp("^(0[0-9]|1[0-9]|2[0-3])[0-5][0-9]$");
        return patt1.test(timeOfDay) || patt2.test(timeOfDay);
    };
    
    // public members
    return {
        drawDigitalClock: drawDigitalClock,
        retrieve24HourTimeString: retrieve24HourTimeString,
        retrieve12HourTimeString: retrieve12HourTimeString,
        validateMilitaryTimeOfDay: validateMilitaryTimeOfDay
    };
    //Self invoke ()
}();
