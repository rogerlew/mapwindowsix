using MapWindow.Projections;

namespace MapWindow.Data
{
    /// <summary>
    /// A very generic interface that is implemented by any dataset, regardless of what kinds of data that it has.
    /// </summary>
    public interface IDataSet
    {
        /// <summary>
        /// Gets or sets a string that describes as clearly as possible what type of
        /// elements are contained in this dataset.
        /// </summary>
        string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a string name identifying this dataset
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the proj-4 string for the projection for this dataset
        /// </summary>
        ProjectionInfo Projection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the space time support for this dataset.
        /// </summary>
        SpaceTimeSupport SpaceTimeSupport
        {
            get;
            set;
        }

        /// <summary>
        /// This closes the data set.  Many times this will simply do nothing, but
        /// in some cases this may close an open connection to a datasource.
        /// </summary>
        void Close();

        
       

    }
}
