namespace MayBee.Serialization.JsonNet
{
    public enum SerializationFormat
    {
        /// <summary>
        /// Serialize maybe values as 'null' (empty) or the value (exists).
        /// </summary>
        Nullable, 
        
        /// <summary>
        /// Serialize maybe values as array of length 0 (empty) or 1 (exists).
        /// </summary>
        Array
    }
}
