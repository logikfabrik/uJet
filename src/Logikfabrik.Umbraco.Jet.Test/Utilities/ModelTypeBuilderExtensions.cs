//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Logikfabrik.Umbraco.Jet.Test.Utilities
//{
//    public static class ModelTypeBuilderExtensions
//    {
//        public static ModelTypeBuilder<T> AsAbstract<T>(this ModelTypeBuilder<T> modelTypeBuilder)
//            where T : TypeModelAttribute
//        {
//            modelTypeBuilder.TypeAttributes = TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Abstract;

//            return modelTypeBuilder;
//        }

//        public static ModelTypeBuilder<T> AsPublic<T>(this ModelTypeBuilder<T> modelTypeBuilder)
//            where T : TypeModelAttribute
//        {
//            modelTypeBuilder.TypeAttributes = TypeAttributes.Class | TypeAttributes.Public;

//            return modelTypeBuilder;
//        }

//        public static ModelTypeBuilder<T> WithPublicConstructor<T>(this ModelTypeBuilder<T> modelTypeBuilder)
//            where T : TypeModelAttribute
//        {
//            modelTypeBuilder.ConstructorAttributes = MethodAttributes.Public;

//            return modelTypeBuilder;
//        }

//        public static ModelTypeBuilder<T> WithPrivateConstructor<T>(this ModelTypeBuilder<T> modelTypeBuilder)
//            where T : TypeModelAttribute
//        {
//            modelTypeBuilder.ConstructorAttributes = MethodAttributes.Private;

//            return modelTypeBuilder;
//        }

//        public static ModelTypeBuilder<T> WithPublicProperty<T>(this ModelTypeBuilder<T> modelTypeBuilder, string propertyName, Type propertyType)
//            where T : TypeModelAttribute
//        {
//        }

//        public static ModelTypeBuilder<T> WithPublicReadOnlyProperty<T>(this ModelTypeBuilder<T> modelTypeBuilder, string propertyName, Type propertyType)
//            where T : TypeModelAttribute
//        {
//        }

//        public static ModelTypeBuilder<T> WithPublicWriteOnlyProperty<T>(this ModelTypeBuilder<T> modelTypeBuilder, string propertyName, Type propertyType)
//            where T : TypeModelAttribute
//        {
//        }

//    }
//}
