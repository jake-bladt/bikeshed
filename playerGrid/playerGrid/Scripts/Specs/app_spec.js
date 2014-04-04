describe("The application", function () {
    it("calculates batting average", function () {
        var ba = app.battingAverage(5.0, 10.0);
        expect(ba).toEqual(0.5);
    });
});
