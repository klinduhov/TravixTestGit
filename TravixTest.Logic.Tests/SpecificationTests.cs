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
            var byPostSpec = new CommentsByPostSpecification(postId);
            

            Assert.False(byPostSpec.IsSatisifiedBy().Compile()(commentFromDifferentPost));
        }

        [Fact]
        public void CommentsPostSpec_IfInputPostIsEqualOneFromComment_ExprIsTrue()
        {
            var postId = Guid.NewGuid();
            Comment commentFromTheSamePost = new Comment(Guid.NewGuid(), postId, "test comment");
            var byPostSpec = new CommentsByPostSpecification(postId);


            Assert.True(byPostSpec.IsSatisifiedBy().Compile()(commentFromTheSamePost));
        }

        [Fact]
        public void AndSpecification_IfByPostExprIsTrueAndOnlyIsReadExprIsFalse_CombinedExprIsFalse()
        {
            var postId = Guid.NewGuid();
            Comment commentFromTheSamePostNotRead = new Comment(Guid.NewGuid(), postId, "test comment");
            var byPostSpec = new CommentsByPostSpecification(postId);
            var onlyIsReadSpec = new OnlyIsReadCommentSpecification();
            var combinedSpecification = byPostSpec.And(onlyIsReadSpec);

            Assert.True(byPostSpec.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
            Assert.False(onlyIsReadSpec.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
            Assert.False(combinedSpecification.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
        }

        [Fact]
        public void AndSpecification_IfByPostExprIsFalseAndOnlyIsReadExprIsTrue_CombinedExprIsFalse()
        {
            Comment commentFromTheSamePostNotRead = new Comment(Guid.NewGuid(), Guid.NewGuid(), "test comment", true);
            var byPostSpec = new CommentsByPostSpecification(Guid.NewGuid());
            var onlyIsReadSpec = new OnlyIsReadCommentSpecification();
            var combinedSpecification = byPostSpec.And(onlyIsReadSpec);

            Assert.False(byPostSpec.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
            Assert.True(onlyIsReadSpec.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
            Assert.False(combinedSpecification.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
        }

        [Fact]
        public void AndSpecification_IfByPostExprIsTrueAndOnlyIsReadExprIsTrue_CombinedExprIsTrue()
        {
            var postId = Guid.NewGuid();
            Comment commentFromTheSamePostNotRead = new Comment(Guid.NewGuid(), postId, "test comment", true);
            var byPostSpec = new CommentsByPostSpecification(postId);
            var onlyIsReadSpec = new OnlyIsReadCommentSpecification();
            var combinedSpecification = byPostSpec.And(onlyIsReadSpec);

            Assert.True(byPostSpec.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
            Assert.True(onlyIsReadSpec.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
            Assert.True(combinedSpecification.IsSatisifiedBy().Compile()(commentFromTheSamePostNotRead));
        }
    }
}