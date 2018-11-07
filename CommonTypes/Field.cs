﻿using System;

namespace CommonTypes
{
    public class Field : IField
    {
        object value;

        public Field(object Value)
        {
            value = Value;
        }

        public Type GetFieldType()
        {
            return value.GetType();
        }


        public object GetValue()
        {
            return value;
        }

        public bool Matches(IField field)
        {

            if (field.GetFieldType().ToString() != this.GetFieldType().ToString())
            {
                return false;

            }

            if (value is StringField)
            {
                StringField myStringField = (StringField)this.value;

                if (myStringField.AnyString)
                {

                    return true;
                }
                if (myStringField.InitialSubString)
                {
                    string myString = myStringField.field;

                    StringField inputStringField = (StringField)field.GetValue();

                    string inputString = inputStringField.field;

                    if (inputString.Substring(0,myString.Length) == myString)
                        return true;
                    return false;

                }

                if (myStringField.FinalSubString)
                {

                    string myString = myStringField.field;

                    StringField inputStringField = (StringField)field.GetValue();

                    string inputString = inputStringField.field;

                    if (inputString.Substring(inputString.Length - myString.Length) == myString)
                        return true;

                    return false;

                }

                else
                {
                    StringField sf = (StringField)value;

                    string s = sf.field;

                    string S = myStringField.field;

                    if(s == S)
                    {
                        return true;
                    }
                    return false;
                }
            }

            if (value is Type)
            {
                Type valueType = (Type)value;

                return field.GetType().ToString() == valueType.ToString();
            }
            else
            {
                /* object[] o = new object[1];

                 Type t = Type.GetType(value.GetType().ToString());


                 return (Boolean)field.GetValue().GetType().GetMethod("Equals").Invoke(field.GetValue(), o);

               */
            }


            return false;
        }
    }
}