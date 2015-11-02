using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Application.CalculatorService
{
    public static class ObjectBuilder
    {
        private static Random rnd = new Random();

        public static object CreateObject(Dictionary<string,double> values) 
        {
            var builder = GetTypeBuilder(rnd.Next());
            foreach (var prop in values)
            {
                 CreateProperty(builder, prop.Key, typeof(double));
            }
           
            var myType = builder.CreateType();
            object instance = Activator.CreateInstance(myType);
            var props = instance.GetType().GetProperties();
            foreach (var prop in props) 
            {
                prop.SetValue(instance,values[prop.Name]);
            }

            return instance;
        }


        private static TypeBuilder GetTypeBuilder(int randomValue)
        {
            AssemblyName an = new AssemblyName("DynamicAssembly" + randomValue.ToString());
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            TypeBuilder tb = moduleBuilder.DefineType("DynamicType"
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(object));
            return tb;
        }

        private static void CreateProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = builder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = builder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            MethodBuilder getPropertyBuiler = CreatePropertyGetter(builder, fieldBuilder);
            MethodBuilder setPropertyBuiler = CreatePropertySetter(builder, fieldBuilder);

            propertyBuilder.SetGetMethod(getPropertyBuiler);
            propertyBuilder.SetSetMethod(setPropertyBuiler);
        }

        private static MethodBuilder CreatePropertyGetter(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            MethodBuilder getMethodBuilder =
                typeBuilder.DefineMethod("get_" + fieldBuilder.Name,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    fieldBuilder.FieldType, Type.EmptyTypes);

            ILGenerator getIL = getMethodBuilder.GetILGenerator();

            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);

            return getMethodBuilder;
        }

        private static MethodBuilder CreatePropertySetter(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            MethodBuilder setMethodBuilder =
                typeBuilder.DefineMethod("set_" + fieldBuilder.Name,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new Type[] { fieldBuilder.FieldType });

            ILGenerator setIL = setMethodBuilder.GetILGenerator();

            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }
    }
}
