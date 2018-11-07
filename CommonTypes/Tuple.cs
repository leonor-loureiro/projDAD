﻿using System;
using CommonTypes;
using System.Collections;


namespace CommonTypes
{
    [Serializable]
    public class Tuple : ITuple
    {
        private ArrayList tuple;

        public Tuple()
        {
            tuple = new ArrayList();
        }

        public Tuple(ArrayList fields)
        {
            tuple = new ArrayList(fields);
        }

        public ITuple Add(Object field)
        {
            tuple.Add(field);
            return this;
        }

        public ArrayList GetFields()
        {
            return tuple;
        }

        public int getLength()
        {
            return tuple.Count;
        }

        public bool Matches(ITuple tuple)
        {
            if (this.getLength() != tuple.getLength())
                return false;

            for (int i = 0; i < this.getLength(); i++)
            {
                Field f1 = (Field)tuple.GetFields()[i];
                Field f2 = (Field)this.tuple[i];


                if (!f2.Matches(f1))
                    return false;
            }

            return true;
        }
    }
}
