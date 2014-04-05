describe("The stat calculator", function () {

    it("exists", function () {
        expect(app.StatCalculator).toBeDefined();
    });

  it("calculates batting averages", function () {
      var ba = app.StatCalculator.battingAverage(5.0, 10.0);
      expect(ba).toEqual(0.5);
  });

  it("calculates earned run averages", function () {
      var era = app.StatCalculator.earnedRunAverage(1.0, 1);
      expect(era).toEqual(9.00);
  });

});