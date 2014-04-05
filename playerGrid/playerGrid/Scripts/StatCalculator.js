(function (app) {
    app.StatCalculator = {
        battingAverage: function (hits, atBats) {
            return hits / atBats;
        },
        earnedRunAverage: function (inningsPitched, earnedRuns) {
            return (earnedRuns / inningsPitched) * 9.0;
        }
    };
})(app);
