namespace Common.MetaSphere.Services
{
    public class MetaSphereShTypedUtilities : MetaSphereShUtilities
    {
        /// <summary>
        /// Finds the current sequence number within the user data and increments it, wrapping if necessary, so that the server accepts the change.
        /// </summary>
        /// <param name="userData">IN/OUT The user data.</param>
        public void IncrementSequenceNumber(ref MetaSwitch.tUserData userData)
        {
            var repositoryData = userData.ShData.RepositoryData;

            repositoryData.SequenceNumber = IncrementSequenceNumber(repositoryData.SequenceNumber);
        }

        public int IncrementSequenceNumber(int currentSeqNumber)
        {

            var newSequenceNumber = currentSeqNumber + 1;

            if (newSequenceNumber > 65535)
            {
                // The sequence number needs to wrap to 1, not 0: 0 is used to create
                // new objects.
                newSequenceNumber = 1;
            }

            return newSequenceNumber;
        }
    }
}
