namespace CollectingProductionDataSystem.Application.ProductionDataServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Application.CalculatorService;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Infrastructure.Contracts;
    public class ProductionDataCalculatorService : IProductionDataCalculatorService
    {
        private readonly ICalculatorService calculator;
        private readonly IProductionData data;

        public ProductionDataCalculatorService(IProductionData dataParam)
        {
            this.calculator = new CalculatorService();
            this.data = dataParam;
        }

        public double Calculate(string formulaCode, FormulaArguments arguments)
        {
            if (string.IsNullOrEmpty(formulaCode))
            {
                throw new ArgumentNullException("The value of the formulaCode can not be null or an empty string!");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("The value of formula arguments can not be null!");
            }

            double result = double.MinValue;
            switch (formulaCode)
            {
                case "P1":
                    result = FormulaP1(arguments);
                    break;
                case "PP1":
                    result = FormulaPP1(arguments);
                    break;
                case "Z1":
                    result = FormulaZ1(arguments);
                    break;
                case "ZZ1":
                    result = FormulaZZ1(arguments);
                    break;
                case "Z2":
                    result = FormulaZ2(arguments);
                    break;
                case "ZZ2":
                    result = FormulaZZ2(arguments);
                    break;
                case "N2":
                    result = FormulaN2(arguments);
                    break;
                case "G2":
                    result = FormulaG2(arguments);
                    break;
                case "V2":
                    result = FormulaV2(arguments);
                    break;
                case "N3":
                    result = FormulaN3(arguments);
                    break;
                case "G3":
                    result = FormulaG3(arguments);
                    break;
                case "V3":
                    result = FormulaV3(arguments);
                    break;
                case "PP3":
                    result = FormulaPP3(arguments);
                    break;
                case "N4":
                    result = FormulaN4(arguments);
                    break;
                case "N5":
                    result = FormulaN5(arguments);
                    break;
                case "G6":
                    result = FormulaG6(arguments);
                    break;
                case "G7":
                    result = FormulaG7(arguments);
                    break;
                case "G7a":
                    result = FormulaG7a(arguments);
                    break;
                case "R10":
                    result = FormulaR10(arguments);
                    break;
                case "O13":
                    result = FormulaO13(arguments);
                    break;
                case "O18":
                    result = FormulaO18(arguments);
                    break;
                case "V14":
                    result = FormulaV14(arguments);
                    break;
                case "I16":
                    result = FormulaI16(arguments);
                    break;
                case "I17":
                    result = FormulaI17(arguments);
                    break;
                case "I17a":
                    result = FormulaI17a(arguments);
                    break;
                case "R18":
                    result = FormulaR18(arguments);
                    break;
                case "P18":
                    result = FormulaP18(arguments);
                    break;
                case "ZZ52":
                    result = FormulaZZ52(arguments);
                    break;
                case "ZZ36":
                    result = FormulaZZ36(arguments);
                    break;
                case "ZZ38":
                    result = FormulaZZ38(arguments);
                    break;
                case "ZZ39":
                    result = FormulaZZ39(arguments);
                    break;
                case "ZZ39a":
                    result = FormulaZZ39a(arguments);
                    break;
                case "Z18":
                    result = FormulaZ18(arguments);
                    break;
                case "N18":
                    result = FormulaN18(arguments);
                    break;
                case "N19":
                    result = FormulaN19(arguments);
                    break;
                case "G19":
                    result = FormulaG19(arguments);
                    break;
                case "G26":
                    result = FormulaG26(arguments);
                    break;
                case "G26a":
                    result = FormulaG26a(arguments);
                    break;
                case "P26":
                    result = FormulaP26(arguments);
                    break;
                case "PP36":
                    result = FormulaPP36(arguments);
                    break;
                case "N42":
                    result = FormulaN42(arguments);
                    break;
                case "C1":
                    result = FormulaC1(arguments);
                    break;
                case "C2":
                    result = FormulaC2(arguments);
                    break;
                case "C3":
                    result = FormulaC3(arguments);
                    break;
                case "C4":
                    result = FormulaC4(arguments);
                    break;
                case "C5":
                    result = FormulaC5(arguments);
                    break;
                case "C6":
                    result = FormulaC6(arguments);
                    break;
                case "C7":
                    result = FormulaC7(arguments);
                    break;
                case "C8":
                    result = FormulaC8(arguments);
                    break;
                default:
                    throw new ArgumentException("The entered value of the formula code is invalid!");
            }

            return result;
        }

        public double Calculate(string expression, string param, Dictionary<string, double> inputParams, string formulaCode)
        {
            var result = calculator.Calculate(expression, param, inputParams.Count, inputParams, formulaCode);
            return result;
        }

        public IEfStatus CalculateByUnitData(int unitDataId, string userName, decimal newValue, decimal oldValue = 0)
        {
            var unitData = this.data.UnitsData.GetById(unitDataId);
            var unitConfig = unitData.UnitConfig;
            if (unitConfig != null)
            {
                var formula = unitConfig.CalculatedFormula;
                var arguments = GetUnitConfigPassportData(unitConfig, unitData, newValue, oldValue);
                var calculatedValue = this.Calculate(formula, arguments);
                AddOrUpdateUnitEnteredForCalculationData(unitDataId, oldValue, newValue);
                AddOrUpdateUnitsManualData(unitDataId, calculatedValue);
                var status = this.data.SaveChanges(userName);

                var relatdUnitConfigs = this.data.RelatedUnitConfigs.All().Where(x => x.RelatedUnitConfigId == unitConfig.Id).ToList();
                if (relatdUnitConfigs.Count() > 0)
                {
                    foreach (var relatedUnitConfig in relatdUnitConfigs)
                    {
                        var rUnitConfig = this.data.UnitConfigs.GetById(relatedUnitConfig.UnitConfigId);
                        if (rUnitConfig.IsCalculated)
                        {
                            var rFormulaCode = rUnitConfig.CalculatedFormula ?? string.Empty;
                            var rArguments = PopulateFormulaTadaFromPassportData(rUnitConfig);
                            PopulateFormulaDataFromRelatedUnitConfigs(rUnitConfig, rArguments, unitData.RecordTimestamp, unitData.ShiftId);
                            var rNewValue = this.Calculate(rFormulaCode, rArguments);
                            status = UpdateCalculatedUnitConfig(relatedUnitConfig.UnitConfigId, rNewValue, unitData.RecordTimestamp, unitData.ShiftId);

                            var subRelatdUnitConfigs = this.data.RelatedUnitConfigs.All().Where(x => x.RelatedUnitConfigId == relatedUnitConfig.UnitConfigId).ToList();
                            if (subRelatdUnitConfigs.Count() > 0)
                            {
                                foreach (var subRelatdUnitConfig in subRelatdUnitConfigs)
                                {
                                    var subRUnitConfig = this.data.UnitConfigs.GetById(subRelatdUnitConfig.UnitConfigId);
                                    if (subRUnitConfig.IsCalculated)
                                    {
                                        var subRFormulaCode = subRUnitConfig.CalculatedFormula ?? string.Empty;
                                        var subRArguments = PopulateFormulaTadaFromPassportData(subRUnitConfig);
                                        PopulateFormulaDataFromRelatedUnitConfigs(subRUnitConfig, subRArguments, unitData.RecordTimestamp, unitData.ShiftId);
                                        var subRNewValue = this.Calculate(subRFormulaCode, subRArguments);
                                        status = UpdateCalculatedUnitConfig(subRelatdUnitConfig.UnitConfigId, subRNewValue, unitData.RecordTimestamp, unitData.ShiftId);
                                    }
                                }
                            }
                        }
                    }

                    status = this.data.SaveChanges(userName);
                }

                return status;
            }
            else
            {
                var validationResults = new List<ValidationResult>();
                validationResults.Add(new ValidationResult("There is not a unit config for that unit data!!!", new[] { "UnitConfig" }));
                return new EfStatus().SetErrors(validationResults);
            }
        }

        private FormulaArguments GetUnitConfigPassportData(UnitConfig unitConfig, UnitsData unitData, decimal newValue, decimal oldValue = 0)
        {
            var arguments = new FormulaArguments();
            arguments.InputValue = (double)newValue;
            arguments.OldValue = (double)oldValue;
            arguments.MaximumFlow = (double?)unitConfig.MaximumFlow;
            arguments.EstimatedDensity = (double?)unitConfig.EstimatedDensity;
            arguments.EstimatedPressure = (double?)unitConfig.EstimatedPressure;
            arguments.EstimatedTemperature = (double?)unitConfig.EstimatedTemperature;
            arguments.EstimatedCompressibilityFactor = (double?)unitConfig.EstimatedCompressibilityFactor;
            arguments.CalculationPercentage = (double?)unitConfig.CalculationPercentage;
            arguments.CustomFormulaExpression = unitConfig.CustomFormulaExpression;
            arguments.Code = unitConfig.Code;
            //arguments.UnitConfigId = unitConfig.Id;
            if (arguments.EstimatedDensity.HasValue)
            {
                var d = ((int)(arguments.EstimatedDensity.Value * 1000)) / 100;
                if (d < 50)
                {
                    d = 50;
                }
                var alpha = this.data.Density2FactorAlphas.All().Where(x => x.Density == d).FirstOrDefault();
                if (alpha != null)
                {
                    arguments.FactorAlpha = (double?)alpha.FactorAlpha;
                }
            }

            var relatedUnitConfigs = unitConfig.RelatedUnitConfigs.ToList();
            foreach (var relatedUnitConfig in relatedUnitConfigs)
            {
                var parameterType = relatedUnitConfig.RelatedUnitConfig.AggregateGroup;
                var inputValue = data.UnitsData
                        .All()
                        .Where(x => x.RecordTimestamp == unitData.RecordTimestamp)
                        .Where(x => x.ShiftId == unitData.ShiftId)
                        .Where(x => x.UnitConfigId == relatedUnitConfig.RelatedUnitConfig.Id)
                        .FirstOrDefault()
                        .RealValue;
                if (parameterType == "I+")
                {
                    arguments.InputValue += inputValue;
                }
                else if (parameterType == "I-")
                {
                    arguments.InputValue -= inputValue;
                }
                else if (parameterType == "T")
                {
                    arguments.Temperature = inputValue;
                }
                else if (parameterType == "P")
                {
                    arguments.Pressure = inputValue;
                }
                else if (parameterType == "D")
                {
                    arguments.Density = inputValue;
                }
                else if (parameterType == "I/")
                {
                    arguments.CalculationPercentage = inputValue;
                }
                else if (parameterType == "I*")
                {
                    arguments.CalculationPercentage = inputValue;
                }
            }
            return arguments;
        }

        private FormulaArguments PopulateFormulaTadaFromPassportData(UnitConfig unitConfig)
        {
            var arguments = new FormulaArguments();
            arguments.MaximumFlow = (double?)unitConfig.MaximumFlow;
            arguments.EstimatedDensity = (double?)unitConfig.EstimatedDensity;
            arguments.EstimatedPressure = (double?)unitConfig.EstimatedPressure;
            arguments.EstimatedTemperature = (double?)unitConfig.EstimatedTemperature;
            arguments.EstimatedCompressibilityFactor = (double?)unitConfig.EstimatedCompressibilityFactor;
            arguments.CalculationPercentage = (double?)unitConfig.CalculationPercentage;
            arguments.CustomFormulaExpression = unitConfig.CustomFormulaExpression;
            arguments.Code = unitConfig.Code;
            //arguments.UnitConfigId = unitConfig.Id;
            return arguments;
        }

        private void PopulateFormulaDataFromRelatedUnitConfigs(UnitConfig unitConfig, FormulaArguments arguments, DateTime recordDate, int shiftId)
        {
            var ruc = unitConfig.RelatedUnitConfigs.ToList();
            foreach (var ru in ruc)
            {
                var parameterType = ru.RelatedUnitConfig.AggregateGroup;
                var inputValue = 0.0;
                //if (ru.RelatedUnitConfigId == model.UnitConfigId)
                //{
                //    inputValue = (double)model.UnitsManualData.Value;
                //}
                //else
                //{
                    var dtExists = data.UnitsData.All()
                        .Where(x => x.RecordTimestamp == recordDate && x.ShiftId == shiftId && x.UnitConfigId == ru.RelatedUnitConfigId)
                        .FirstOrDefault();

                    inputValue = dtExists == null ? 0.0D : dtExists.RealValue;
                //}

                if (parameterType == "I+")
                {
                    var exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
                    arguments.InputValue = exsistingValue + inputValue;
                }
                else if (parameterType == "I-")
                {
                    var exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
                    arguments.InputValue = exsistingValue - inputValue;
                }
                else if (parameterType == "T")
                {
                    arguments.Temperature = inputValue;
                }
                else if (parameterType == "P")
                {
                    arguments.Pressure = inputValue;
                }
                else if (parameterType == "D")
                {
                    arguments.Density = inputValue;
                }
                else if (parameterType == "I/")
                {
                    arguments.CalculationPercentage = inputValue;
                }
                else if (parameterType == "I*")
                {
                    arguments.CalculationPercentage = inputValue;
                }
            }
        }

        private IEfStatus UpdateCalculatedUnitConfig(int unitConfigId, double newValue, DateTime recordDate, int shiftId)
        {
            var record = data.UnitsData
                               .All().Include(x => x.UnitConfig)
                               .Where(x => x.RecordTimestamp == recordDate && x.ShiftId == shiftId && x.UnitConfigId == unitConfigId)
                               .FirstOrDefault();

            IEfStatus result = new EfStatus();
            var existManualRecord = this.data.UnitsManualData.All().FirstOrDefault(x => x.Id == record.Id);
            if (existManualRecord == null)
            {
                this.data.UnitsManualData.Add(new UnitsManualData
                {
                    Id = record.Id,
                    Value = (decimal)newValue,
                    EditReasonId = this.data.EditReasons.All().Where(x => x.Name == "Калкулация").FirstOrDefault().Id
                });
            }
            else
            {
                existManualRecord.Value = (decimal)newValue;
                existManualRecord.EditReasonId = this.data.EditReasons.All().Where(x => x.Name == "Калкулация").FirstOrDefault().Id;
                this.data.UnitsManualData.Update(existManualRecord);
            }

            return result;
        }

        private void AddOrUpdateUnitsManualData(int unitDataId, double calculatedValue)
        {
            var newManualRecord = new UnitsManualData
            {
                Id = unitDataId,
                Value = (decimal)calculatedValue,
                EditReasonId = this.data.EditReasons.All().Where(x => x.Name == "Калкулация").FirstOrDefault().Id
            };
            var existManualRecord = this.data.UnitsManualData.All().FirstOrDefault(x => x.Id == newManualRecord.Id);
            if (existManualRecord == null)
            {
                this.data.UnitsManualData.Add(newManualRecord);
            }
            else
            {
                existManualRecord.Value = (decimal)calculatedValue;
                this.data.UnitsManualData.Update(existManualRecord);
            }
        }

        private void AddOrUpdateUnitEnteredForCalculationData(int unitDataId, decimal oldValue, decimal newValue)
        {
            if (oldValue < 0)
            {
                var totalizerMaximumValue = this.data.UnitsData.All().Include(x => x.UnitConfig).Where(x => x.Id == unitDataId).Select(x => x.UnitConfig.EstimatedCompressibilityFactor).FirstOrDefault();
                var diff = totalizerMaximumValue.Value + oldValue;
                oldValue = diff;
            }


            var newUnitEnteredForCalculationRecord = new UnitEnteredForCalculationData
            {
                UnitDataId = unitDataId,
                OldValue = oldValue,
                NewValue = newValue
            };
            //var existsUnitEnteredForCalculationRecord = this.data.UnitEnteredForCalculationDatas.All().Where(x => x.Id == newUnitEnteredForCalculationRecord.Id).FirstOrDefault();
            //if (existsUnitEnteredForCalculationRecord == null)
            {
                this.data.UnitEnteredForCalculationDatas.Add(newUnitEnteredForCalculationRecord);
            }
            //else
            //{
            //    existsUnitEnteredForCalculationRecord.OldValue = oldValue;
            //    existsUnitEnteredForCalculationRecord.NewValue = newValue;
            //    this.data.UnitEnteredForCalculationDatas.Update(existsUnitEnteredForCalculationRecord);
            //}
        }

        ///  <summary>
        /// 1) P1 ;ПАРА-КОНСУМИРАНА [ТОНОВЕ] :: X A1,F Q
        ///  </summary>
        private double FormulaP1(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double d2 = args.MaximumFlow.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;

            double a1 = Functions.GetValueFormulaA1(p, t, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a1);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            double result = calculator.Calculate("par.f", "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 2) PP1 ;ПАРА-ПРОИЗВЕДЕНА [ТОНОВЕ] :: X A1,F Q
        /// </summary>
        private double FormulaPP1(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure.Value;
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature.Value;
            }

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;

            double a1 = Functions.GetValueFormulaA1(p, t, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a1);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            double result = calculator.Calculate("par.f", "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 3) Z1 ;ПАРА-КОНСУМИРАНА [MWH] :: X A1,F,K15,K2,K3,EN S Q=Q*ENT/0.860 Q
        /// </summary>
        private double FormulaZ1(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double d2 = args.MaximumFlow.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;

            double a1 = Functions.GetValueFormulaA1(p, t, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a1);
            double ent = Functions.GetValueFormulaEN(t, p);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            inputParams.Add("ent", ent);
            string expr = @"(par.f*par.ent)/0.860";
            double result = calculator.Calculate(expr, "par", 2, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 4) ZZ1 ;ПАРА-ПРОИЗВЕДЕНА [MWH] :: X A1,F,K15,K2,K3,EN S Q=Q*ENT/0.860 Q
        /// </summary>
        private double FormulaZZ1(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double d2 = args.MaximumFlow.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;

            double a1 = Functions.GetValueFormulaA1(p, t, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a1);
            double ent = Functions.GetValueFormulaEN(t, p);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            inputParams.Add("ent", ent);
            string expr = @"(par.f*par.ent)/0.860";
            var result = calculator.Calculate(expr, "par", 2, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 5) Z2 ;ПАРА-КОНСУМИРАНА, АСУТП, [MWH] :: X K15,K2,K3,EN S Q=PL*ENT/0.860 Q
        /// </summary>
        private double FormulaZ2(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value > 0 ? args.Pressure.Value : args.EstimatedPressure.Value;
            double t = args.Temperature.Value > 0 ? args.Temperature.Value : args.EstimatedTemperature.Value;

            double ent = Functions.GetValueFormulaEN(t, p);
            if (ent < 0)
            {
                ent = Functions.GetValueFormulaEN(1, 1);
            }

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("pl", pl);
            inputParams.Add("ent", ent);

            string expr = @"(par.pl*par.ent)/0.860";
            var result = calculator.Calculate(expr, "par", 2, inputParams, args.Code);
            return result;
        }

        /// <summary>
        ///  6) ZZ2 ;ПАРА-ПРОИЗВЕДЕНА, АСУТП, [MWH] :: X K15,K2,K3,EN S Q=PL*ENT/0.860 Q
        /// </summary>
        private double FormulaZZ2(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of Pressure(P) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of Temperature(T) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value > 0 ? args.Pressure.Value : args.EstimatedPressure.Value;
            double t = args.Temperature.Value > 0 ? args.Temperature.Value : args.EstimatedTemperature.Value;

            double ent = Functions.GetValueFormulaEN(t, p);
            if (ent < 0)
            {
                ent = Functions.GetValueFormulaEN(1, 1);
            }

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("pl", pl);
            inputParams.Add("ent", ent);

            string expr = @"(par.pl*par.ent)/0.860";
            double result = calculator.Calculate(expr, "par", 2, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 7) N2 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ ;ИЗЧИСЛЯВАНЕ НА ПЛЪТНОСТ :: S:D<0.5 D=0.5 X C,A2,F Q
        /// </summary>
        private double FormulaN2(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }
            if (!args.FactorAlpha.HasValue)
            {
                throw new ArgumentNullException("The value of FactorAlpha(AL) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;
            double d4 = args.EstimatedDensity.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;

            double d = args.Density.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double al = args.FactorAlpha.Value;

            if (d < 0.5)
            {
                d = 0.5;
            }

            double c = Functions.GetValueFormulaC(t, d, al);
            double a2 = Functions.GetValueFormulaA2(p, t, c, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 8) G2 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД :: S DF=D X A2,F Q
        /// </summary>
        private double FormulaG2(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;
            double d4 = args.EstimatedDensity.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double d = args.Density.Value;

            double a2 = Functions.GetValueFormulaA2(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 9) V2 ;КОНДЕНЗАТ И ХОВ :: X C1,A2,F Q
        /// </summary>
        private double FormulaV2(FormulaArguments args)
        {
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double qpt3 = 0.9787;
            double a2 = Functions.GetValueFormulaA2(p, t, qpt3, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 10) N3 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: S:D<0.5 D=0.5 X C,A7,F S Q=Q*DF Q
        /// </summary>
        private double FormulaN3(FormulaArguments args)
        {
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }
            double c = Functions.GetValueFormulaC(t, d, al);
            double a7 = Functions.GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a7);
            f = f * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 11) G3 ;НЕФТОЗАВОДСКИ ГАЗ :: S DF=D X A7,F S Q=Q*DF Q
        /// </summary>
        private double FormulaG3(FormulaArguments args)
        {
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double df = d;

            double a7 = Functions.GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a7);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 12) G3 ;КОНДЕНЗАТ И ХОВ :: X C1,A7,F S Q=Q*DF Q
        /// </summary>
        private double FormulaV3(FormulaArguments args)
        {
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double qpt3 = 0.9929;
            double df = qpt3;

            //double c1 = Functions.GetValueFormulaC1(p, t, qpt3);
            double a7 = Functions.GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a7);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 13) PP3 ;МРЕЖОВА ВОДА :: X C1,A7,F S Q=Q*DF Q
        /// </summary>
        private double FormulaPP3(FormulaArguments args)
        {
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;
            double qpt3 = 0.9929;
            double df = qpt3;

            //double c1 = Functions.GetValueFormulaC1(p, t, qpt3);
            double a7 = Functions.GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a7);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 14) N4 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: S:D<0.5 D=0.5 X C,A3,F Q
        /// </summary>
        private double FormulaN4(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }
            if (!args.FactorAlpha.HasValue)
            {
                throw new ArgumentNullException("The value of FactorAlpha(AL) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;
            double d4 = args.EstimatedDensity.Value;
            double d6 = args.EstimatedTemperature.Value;
            double d = args.Density.Value;
            double t = args.Temperature.Value;
            double al = args.FactorAlpha.Value;
            if (d < 0.5)
            {
                d = 0.5;
            }

            double c = Functions.GetValueFormulaC(t, d, al);
            double a3 = Functions.GetValueFormulaA3(t, c, d4, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a3);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 15) N5 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: S:D<0.5 D=0.5 X C,A15,F S Q=Q*DF Q
        /// </summary>
        private double FormulaN5(FormulaArguments args)
        {
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d6 = 50;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }

            double df = Functions.GetValueFormulaC(t, d, al);
            double a15 = Functions.GetValueFormulaA15(t, df, d4, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a15);
            f = f * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 16) G6 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД :: X A4,F S Q=Q/1000 Q
        /// </summary>
        private double FormulaG6(FormulaArguments args)
        {
            double p = 15;
            double pl = 20;
            double d2 = 15;
            double d5 = 10;

            double a4 = Functions.GetValueFormulaA4(p, d5);
            double f = Functions.GetValueFormulaF(d2, pl, a4);
            f = f / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 17) G7 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД :: S DF=D X A7,F S Q=Q*DF/1000 Q
        /// </summary>
        private double FormulaG7(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity.Value;
            }

            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure.Value;
            }

            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature.Value;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;
            double d = args.Density.Value;
            double d2 = args.MaximumFlow.Value;
            double d4 = args.EstimatedDensity.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;
            double df = d;

            double a7 = Functions.GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a7);
            f = (f * df) / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 9917) G7 ;НЕФТОЗАВОДСКИ ГАЗ И ВОДОРОД НОРМАЛНИ КУБИЧНИ МЕТРИ
        /// </summary>
        private double FormulaG7a(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity.Value;
            }

            double pl = args.InputValue.Value;
            double d = args.Density.Value;
            double d2 = args.MaximumFlow.Value;
            double d4 = args.EstimatedDensity.Value;
            double df = d;

            double f = (((d2 * pl) / 100) * 8) * d4;
            f = (f * df) / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 20) R10 ;ПРИРОДЕН ГАЗ :: X A7,F S Q=Q/1000 Q
        /// </summary>
        private double FormulaR10(FormulaArguments args)
        {
            double p = 15;
            double t = 40;
            double pl = 20;
            double d2 = 15;
            double d4 = 5;
            double d5 = 10;
            double d6 = 50;
            double d = 50;

            double a7 = Functions.GetValueFormulaA7(p, t, d, d4, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a7);
            f = f / 1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 23) O13 ;ОБОРОТНИ, ПИТЕЙНИ И ОТПАДНИ ВОДИ :: X A10 Q
        /// </summary>
        private double FormulaO13(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }

            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;

            double q = Functions.GetValueFormulaA10(pl, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 36) O18 ;;БРОЯЧИ ЗА ВОДИ :: X A11 Q
        /// </summary>
        private double FormulaO18(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of Old CounterIndication(PL-1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 26) V14 ;КОНДЕНЗАТ, ХОВ :: X A10 Q
        /// </summary>
        private double FormulaV14(FormulaArguments args)
        {
            double pl = 20;
            double d2 = 15;

            double q = Functions.GetValueFormulaA10(pl, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 30) I16 ;ВЪЗДУХ, АЗОТ, КИСЛОРОД :: X A1,F Q
        /// </summary>
        private double FormulaI16(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double p = args.Pressure.Value;
            double t = args.Temperature.Value;

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;
            double d5 = args.EstimatedPressure.Value;
            double d6 = args.EstimatedTemperature.Value;

            double a1 = Functions.GetValueFormulaA1(p, t, d5, d6);
            double f = Functions.GetValueFormulaF(d2, pl, a1);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 31) I17 ;ВЪЗДУХ, АЗОТ, КИСЛОРОД :: X A4,F Q
        /// </summary>
        private double FormulaI17(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }

            double pl = args.InputValue.Value;
            double p = args.Pressure.Value;
            double d2 = args.MaximumFlow.Value;
            double d5 = args.EstimatedPressure.Value;

            double a4 = Functions.GetValueFormulaA4(p, d5);
            double f = Functions.GetValueFormulaF(d2, pl, a4);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);

            string expr = @"par.f";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 9931) I17a ;ВЪЗДУХ, АЗОТ, КИСЛОРОД ОТ НОРМАЛНИ КУБИЧНИ МЕТРИ
        /// </summary>
        private double FormulaI17a(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.CalculationPercentage.HasValue)
            {
                 throw new ArgumentNullException("The value of CalculationPercentage is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double d2 = args.MaximumFlow.Value;
            double c = args.CalculationPercentage.Value;
            double f = ((pl / 100.00) * d2) * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("f", f);
            string expr = @"par.f";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 32) R18 ;БРОЯЧИ ЗА ПРИРОДЕН ГАЗ :: X A11 Q
        /// </summary>
        private double FormulaR18(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of Old CounterIndication(PL-1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 39) P18 ;;БРОЯЧИ ЗА ПАРА :: X A11 Q
        /// </summary>
        private double FormulaP18(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of Old CounterIndication(PL-1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 41) ZZ52 ;БРОЯЧИ ЗА ПАРА - ДОБАВЕНО 03/05/2007 - ЗА ТЕЦА /ТЦ104/  :: X A11 Q
        /// </summary>
        private double FormulaZZ52(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of Old CounterIndication(PL-1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 42) Z18 ;БРОЯЧИ ЗА ПАРА [MWH] :: X A11,K15,K2,K3,EN S Q=Q*ENT/0.860 Q
        /// </summary>
        private double FormulaZ18(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of OldValue(PL1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedPressure.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.Pressure.HasValue)
            {
                args.Pressure = args.EstimatedPressure;
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;
            double p = args.Pressure.Value;
            double t = args.Temperature.Value;

            double a11 = Functions.GetValueFormulaA11(pl, pl1, d2);
            double ent = Functions.GetValueFormulaEN(t, p);
            double q = (a11 * ent) / 0.860;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"(par.q)";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 25) N14 ;ТЕЧНИ НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: X A10 Q
        /// </summary>
        public double FormulaN14(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            var pl = args.InputValue.Value;
            var d2 = args.MaximumFlow.Value;
            var q = Functions.GetValueFormulaA10(pl, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// // 34) N18 ;;БРОЯЧИ ЗА НЕФТОПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ :: X A11 Q
        /// </summary>
        public double FormulaN18(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of Old CounterIndication(PL-1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);
            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 47) N19 ;БРОЯЧИ ЗА НЕФТОПРОДУКТИ :: S:D<0.5 D=0.5 X C,A11 S Q=Q*DF Q
        /// </summary>
        private double FormulaN19(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of Old CounterIndication(PL-1) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedPressure(D5) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }
            if (!args.FactorAlpha.HasValue)
            {
                throw new ArgumentNullException("The value of FactorAlpha(AL) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double t = args.Temperature.Value;
            double d = args.Density.Value;
            double d2 = args.MaximumFlow.Value;
            double al = args.FactorAlpha.Value;
            if (d < 0.5)
            {
                d = 0.5;
            }
            double c = Functions.GetValueFormulaC(t, d, al);
            double a11 = Functions.GetValueFormulaA11(pl, pl1, d2);
            double q = a11 * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        ///  48) G19 ;БРОЯЧИ ЗА ГАЗОВЕ :: S DF=D X A11 S Q=Q*DF Q
        /// </summary>
        private double FormulaG19(FormulaArguments args)
        {
            double pl = 20;
            double pl1 = 10;
            double d = 50;
            double d2 = 15;
            double df = d;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);
            q = q * df;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 62) G26 ;ПРЕВРЪЩАНЕ ДИМЕНСИЯТА ОТ "Н.M.КУБ." В "ТОНОВЕ" :: S Q=PL*D Q
        /// </summary>
        private double FormulaG26(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of Density(D) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double d = args.EstimatedDensity.Value;
            double q = pl * d;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 62) G26 ;ПРЕВРЪЩАНЕ ДИМЕНСИЯТА ОТ "Н.CM.КУБ." В "ТОНОВЕ" :: S Q=PL*D Q
        /// </summary>
        private double FormulaG26a(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of Density(D) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double d = args.EstimatedDensity.Value;
            double q = (pl * d)/1000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 63) P26 ;ПРЕВРЪЩАНЕ ДИМЕНСИЯТА ОТ "M.КУБ." В "ТОНОВЕ" :: S Q=PL*D Q
        /// </summary>
        private double FormulaP26(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of Density(D) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }

            double pl = args.InputValue.Value;
            double d = args.Density.Value;
            double q = pl * d;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 79) PP36 ;МРЕЖ.ВОДА КАТО ПАРА /ТОН/ :: S Q=PL*T*D(2)/73500 Q
        /// </summary>
        public double FormulaPP36(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double t = args.Temperature.Value;
            double d2 = args.MaximumFlow.Value;
            double q = pl * t * d2 / 73500;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 80) ZZ36 ;МРЕЖ.ВOДА КАТО ПАРА [MWH] :: S Q=PL*T*D(2)/86000 Q
        /// </summary>
        private double FormulaZZ36(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double t = args.EstimatedTemperature.Value;
            double d2 = args.MaximumFlow.Value;

            double q = pl * t * d2 / 86000;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";

            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 82) ZZ38 ;СТОКОВ КОНДЕНЗАТ-ТОПЛОЕНЕРГИЯ /Х.КВТЧ./ :: S Q=PL/0.860 Q
        /// </summary>
        public double FormulaZZ38(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }

            double pl = args.InputValue.Value;
            double q = pl / 860;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 84) ZZ39 ;КОНДЕНЗАТ-ТОПЛОЕНЕРГИЯ /Х.КВТЧ/ :: S DF=D X A11 S Q=Q*DF*T/860 Q
        /// </summary>
        public double FormulaZZ39(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.OldValue.HasValue)
            {
                throw new ArgumentNullException("The value of OldValue(PL-1) is not allowed to be null");
            }
            if (!args.MaximumFlow.HasValue)
            {
                throw new ArgumentNullException("The value of MaximumFlow(D2) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double pl1 = args.OldValue.Value;
            double d2 = args.MaximumFlow.Value;
            double t = args.Temperature.Value;
            double d = args.Density.Value;
            double df = d;

            double q = Functions.GetValueFormulaA11(pl, pl1, d2);
            q = q * df * (t/860);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 84) ZZ39 ;КОНДЕНЗАТ-ТОПЛОЕНЕРГИЯ /Х.КВТЧ/ :: S DF=D X A11 S Q=Q*DF*T/860 Q
        /// </summary>
        public double FormulaZZ39a(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.EstimatedDensity.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedDensity(D4) is not allowed to be null");
            }
            if (!args.Density.HasValue)
            {
                args.Density = args.EstimatedDensity;
            }
            if (!args.EstimatedTemperature.HasValue)
            {
                throw new ArgumentNullException("The value of EstimatedTemperature(D6) is not allowed to be null");
            }
            if (!args.Temperature.HasValue)
            {
                args.Temperature = args.EstimatedTemperature;
            }

            double pl = args.InputValue.Value;
            double t = args.Temperature.Value;
            double d = args.Density.Value;
            double df = d;

            double q = pl * df * (t/860);

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// 87) N42 ;НЕФТОПРОДУКТИ /ХО-1/ :: S:D<0.5 D=0.5 X C S Q=PL*DF Q
        /// </summary>
        private double FormulaN42(FormulaArguments args)
        {
            double pl = 20;
            double t = 40;
            double d = 50;
            double al = 0.001163;
            if (d < 0.5)
            {
                d = 0.5;
            }
            double c = Functions.GetValueFormulaC(t, d, al);
            double q = pl * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", q);

            string expr = @"par.q";
            double result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF1 - зчисляване на колко е стойността на % от число
        /// </summary>
        public double FormulaC1(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.CalculationPercentage.HasValue)
            {
                throw new ArgumentNullException("The value of Calculation percentage is not allowed to be null");
            }
            if (args.CalculationPercentage.Value < 0d || args.CalculationPercentage.Value > 100d)
            {
                throw new ArgumentOutOfRangeException("The value of Calculation percentage must be a double number between 0 and 100");
            }

            var pl = args.InputValue.Value;
            var c = args.CalculationPercentage.Value;
            var r = (c / 100.00) * pl;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", r);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF2 - визуализира входното показание като резултат от формулата
        /// </summary>
        public double FormulaC2(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }

            var pl = args.InputValue.Value;
            var r =  pl;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", r);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF3 - калкулира  входното показание по броя на часовете в една смяна
        /// </summary>
        public double FormulaC3(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }

            var pl = args.InputValue.Value;
            var r =  pl * 8;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", r);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF4 - калкулира  входното показание по броя разделено на 1000
        /// </summary>
        public double FormulaC4(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }

            var pl = args.InputValue.Value;
            var r =  pl / 1000.00d;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", r);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF5 - изчислява въведената стойност умножена по стойност записана в колоната за процент
        /// </summary>
        public double FormulaC5(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.CalculationPercentage.HasValue)
            {
                throw new ArgumentNullException("The value of Calculation percentage is not allowed to be null");
            }

            var pl = args.InputValue.Value;
            var c = args.CalculationPercentage.Value;
            var r = pl * c;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", r);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF6 - коефициента от конфигурационната таблица като стойност на формулата.
        /// </summary>
        public double FormulaC6(FormulaArguments args)
        {
            if (!args.CalculationPercentage.HasValue)
            {
                throw new ArgumentNullException("The value of Calculation percentage is not allowed to be null");
            }

            var c = args.CalculationPercentage.Value;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", c);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            return result;
        }

        /// <summary>
        /// UCF7 - изчислява въведената стойност разделена по стойност записана в колоната за процент
        /// </summary>
        public double FormulaC7(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (!args.CalculationPercentage.HasValue)
            {
                throw new ArgumentNullException("The value of Calculation percentage is not allowed to be null");
            }

            var x = args.InputValue.Value;
            var y = args.CalculationPercentage.Value;
            var d = x / y;

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("q", d);

            string expr = @"par.q";
            var result = calculator.Calculate(expr, "par", 1, inputParams, args.Code);
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                result = 0.0;
            }
            return result;
        }

        /// <summary>
        /// UCF7 - изчислява въведената стойност като аргумент на формула
        /// </summary>
        public double FormulaC8(FormulaArguments args)
        {
            if (!args.InputValue.HasValue)
            {
                throw new ArgumentNullException("The value of CounterIndication(PL) is not allowed to be null");
            }
            if (String.IsNullOrEmpty(args.CustomFormulaExpression))
            {
                throw new ArgumentNullException("The value of CustomFormulaExpression is not allowed to be null");
            }

            var inputParams = new Dictionary<string, double>();
            inputParams.Add("p0", args.InputValue.Value);
            string expr = @args.CustomFormulaExpression;
            var result = calculator.Calculate(expr, "p", 1, inputParams, args.Code);
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                result = 0.0;
            }
            return result;
        }
    }
}