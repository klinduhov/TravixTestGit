using System;
using TravixTest.Logic.DomainModels;
using TravixTest.Logic.Specifications;
using Xunit;

namespace TravixTest.Logic.Tests
{
    public class SpecificationTests
    {
        [Fact]
        public void CommentsPostSpec_IfInputPostIsNotEqualOneFromComment_ExprIsFalse()
        {
            var postId = Guid.NewGuid();
            Comment commentFromDifferentPost = new Comment(Guid.NewGuid(), Guid.NewGuid(), "test comment");
            var ifPostSpec = new CommentsByPostSpecification(postId);
            

            Assert.False(ifPostSpec.IsSatisifiedBy().Compile()(commentFromDifferentPost));
        }

        [Fact]
        public void CommentsPostSpec_IfInputPostIsEqualOneFromComment_ExprIsTrue()
        {
            var postId = Guid.NewGuid();
            Comment commentFromDifferentPost = new Comment(Guid.NewGuid(), postId, "test comment");
            var ifPostSpec = new CommentsByPostSpecification(postId);


            Assert.True(ifPostSpec.IsSatisifiedBy().Compile()(commentFromDifferentPost));
        }

        [Fact]
        public void AndSpecification_IfByPostAndOnlyIsReadSpecsCombined_FinalExprContainsBothSpecsExpressionByAnd()
        {
        }
    }
}