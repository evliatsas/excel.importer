namespace Iris.Importer
{
    /// <summary>
    /// Type used to hold lookup values
    /// </summary>
    public class Lookup<TKey>
    {
        /// <summary>
        /// The ID value of the Instance
        /// </summary>
        public TKey Id { get; set; }
        /// <summary>
        /// The Description Tag of the Instance
        /// </summary>
        public string Description { get; set; }

        public bool Equals(Lookup<TKey> profile)
        {
            return this.GetHashCode() == profile.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is Lookup<TKey>)
                return Equals(obj as Lookup<TKey>);
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode();
            }
        }
    }

    public class Lookup : Lookup<string>
    {

    }

    public class LookupExt<TKey, TValue> : Lookup<TKey>
    {
        /// <summary>
        /// The Value of the Extended Lookup
        /// </summary>
        public TValue Value { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var val = obj as LookupExt<TKey, TValue>;
            if (val == null)
            {
                return false;
            }
            else
            {
                return Equals(val.Id, this.Id) && Equals(val.Value, this.Value);
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                string key = string.Format("{0}-{1}",
                                        this.Id.ToString(),
                                        this.Value.ToString());

                return key.GetHashCode();
            }
        }
    }

    public class LookupExt<TValue> : Lookup<string>
    {
        /// <summary>
        /// The Value of the Extended Lookup
        /// </summary>
        public TValue Value { get; set; }
    }
}
