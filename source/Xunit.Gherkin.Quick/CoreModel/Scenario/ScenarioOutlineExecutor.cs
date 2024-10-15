using System;
using System.Threading.Tasks;

namespace Xunit.Gherkin.Quick
{
    internal sealed class ScenarioOutlineExecutor
    {
        private readonly IFeatureFileRepository _featureFileRepository;

        public ScenarioOutlineExecutor(IFeatureFileRepository featureFileRepository)
        {
             _featureFileRepository = featureFileRepository ?? throw new ArgumentNullException(nameof(featureFileRepository));
        }
    }
}
