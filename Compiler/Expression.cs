using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class TemplateItem{
public string Template;
public string Contract;
}
public partial class DicEle{
public string key;
public string value;
public string text;
}
public partial class LiteLangVisitor{
public  override  object @base( VariableStatementContext context ){
var obj = "";
var r1 = ((Result)(Visit(context.idExpression())));
var r2 = ((Result)(Visit(context.expression())));
if ( context.typeType()!=null ) {
var Type = ((string)(Visit(context.typeType())));
obj = (new System.Text.StringBuilder("").Append(Type).Append(" ").Append(r1.text).Append(" = ").Append(r2.text).Append("")).to_str()+Terminate+Wrap;
}
else {
obj = run(()=>{if ( r1.isDefine||r1.text==this.selfID||r1.text==this.superID||r1.text==setID ) {
return (new System.Text.StringBuilder("").Append(r1.text).Append(" = ").Append(r2.text).Append("")).to_str()+Terminate+Wrap;}
else {
return (new System.Text.StringBuilder("var ").Append(r1.text).Append(" = ").Append(r2.text).Append("")).to_str()+Terminate+Wrap;}
});
}
return obj;
}
public  override  object @base( VariableDeclaredStatementContext context ){
var obj = "";
var Type = ((string)(Visit(context.typeType())));
var r = ((Result)(Visit(context.idExpression())));
obj = (new System.Text.StringBuilder("").Append(Type).Append(" ").Append(r.text).Append("")).to_str()+Terminate+Wrap;
return obj;
}
public  override  object @base( AssignStatementContext context ){
var r1 = ((Result)(Visit(context.tupleExpression(0))));
var r2 = ((Result)(Visit(context.tupleExpression(1))));
var obj = r1.text+Visit(context.assign())+r2.text+Terminate+Wrap;
return obj;
}
public  override  object @base( AssignContext context ){
if ( context.op.Type==Mod_Equal ) {
return "%=";
}
return context.op.Text;
}
public  override  object @base( ExpressionStatementContext context ){
var r = ((Result)(Visit(context.expression())));
return r.text+Terminate+Wrap;
}
public  override  object @base( ExpressionContext context ){
var count = context.ChildCount;
var r = (new Result());
switch (count) {
case 3 :
{ var e1 = ((Result)(Visit(context.GetChild(0))));
var e2 = Visit(context.GetChild(2));
var op = Visit(context.GetChild(1));
switch (context.GetChild(1)) {
case JudgeContext it :
{ r.data=Bool;
}break;
case AddContext it :
{ r.data=run(()=>{if ( ((string)(e1.data))==Str||((string)(((Result)(e2)).data))==Str ) {
return Str;}
else if ( ((string)(e1.data))==I32&&((string)(((Result)(e2)).data))==I32 ) {
return I32;}
else {
return F64;}
});
}break;
case MulContext it :
{ r.data=run(()=>{if ( ((string)(e1.data))==I32&&((string)(((Result)(e2)).data))==I32 ) {
return I32;}
else {
return F64;}
});
}break;
case PowContext it :
{ r.data=F64;
switch (op) {
case "**" :
{ op = "pow";
}break;
case "//" :
{ op = "root";
}break;
case "\\\\" :
{ op = "log";
}break;
}
r.text=(new System.Text.StringBuilder("").Append(op).Append("(").Append(e1.text).Append(", ").Append(((Result)(e2)).text).Append(")")).to_str();
return r;
}break;
}
r.text=e1.text+op+((Result)(e2)).text;
}break;
case 2 :
{ r = ((Result)(Visit(context.GetChild(0))));
if ( context.GetChild(1).GetType()==@typeof<TypeConversionContext>() ) {
var e2 = ((string)(Visit(context.GetChild(1))));
r.data=e2;
r.text=(new System.Text.StringBuilder("((").Append(e2).Append(")(").Append(r.text).Append("))")).to_str();
}
else if ( context.GetChild(1).GetType()==@typeof<CallExpressionContext>() ) {
var e2 = ((Result)(Visit(context.GetChild(1))));
r.text=r.text+e2.text;
}
else if ( context.GetChild(1).GetType()==@typeof<CallFuncContext>() ) {
var e2 = ((Result)(Visit(context.GetChild(1))));
r.text=r.text+e2.text;
}
else if ( context.GetChild(1).GetType()==@typeof<CallElementContext>() ) {
var e2 = ((Result)(Visit(context.GetChild(1))));
r.text=r.text+e2.text;
}
else if ( context.GetChild(1).GetType()==@typeof<JudgeCaseExpressionContext>() ) {
var e2 = ((Result)(Visit(context.GetChild(1))));
r.text=(new System.Text.StringBuilder("run(()=> { switch (").Append(r.text).Append(") ").Append(e2.text).Append("})")).to_str();
}
else {
if ( context.op.Type==LiteParser.Bang ) {
r.text=(new System.Text.StringBuilder("ref ").Append(r.text).Append("")).to_str();
}
else if ( context.op.Type==LiteParser.Question ) {
r.text+="?";
}
}
}break;
case 1 :
{ r = ((Result)(Visit(context.GetChild(0))));
}break;
}
return r;
}
public  override  object @base( TypeConversionContext context ){
return ((string)(Visit(context.typeType())));
}
public  override  object @base( CallContext context ){
return context.op.Text;
}
public  override  object @base( WaveContext context ){
return context.op.Text;
}
public  override  object @base( JudgeTypeContext context ){
return context.op.Text;
}
public  override  object @base( BitwiseContext context ){
return ((string)(this.Visit(context.GetChild(0))));
}
public  override  object @base( BitwiseAndContext context ){
return "&";
}
public  override  object @base( BitwiseOrContext context ){
return "|";
}
public  override  object @base( BitwiseXorContext context ){
return "^";
}
public  override  object @base( BitwiseLeftShiftContext context ){
return "<<";
}
public  override  object @base( BitwiseRightShiftContext context ){
return ">>";
}
public  override  object @base( JudgeContext context ){
if ( context.op.Type==Not_Equal ) {
return "!=";
}
else if ( context.op.Type==And ) {
return "&&";
}
else if ( context.op.Type==Or ) {
return "||";
}
return context.op.Text;
}
public  override  object @base( AddContext context ){
return context.op.Text;
}
public  override  object @base( MulContext context ){
if ( context.op.Type==Mod ) {
return "%";
}
return context.op.Text;
}
public  override  object @base( PowContext context ){
return context.op.Text;
}
public  override  object @base( PrimaryExpressionContext context ){
if ( context.ChildCount==1 ) {
var c = context.GetChild(0);
if ( c.@is<DataStatementContext>() ) {
return Visit(context.dataStatement());
}
else if ( c.@is<IdContext>() ) {
return Visit(context.id());
}
else if ( context.t.Type==Discard ) {
return (new Result(){text = "_",data = "var"});
}
}
else if ( context.ChildCount==2 ) {
var id = ((Result)(Visit(context.id())));
var template = ((string)(Visit(context.templateCall())));
return (new Result(){text = id.text+template,data = id.text+template});
}
var r = ((Result)(Visit(context.expression())));
return (new Result(){text = "("+r.text+")",data = r.data});
}
public  override  object @base( ExpressionListContext context ){
var r = (new Result());
var obj = "";
foreach (var i in range(0,context.expression().Length,1,true,false)){
var temp = ((Result)(Visit(context.expression(i))));
obj+=run(()=>{if ( i==0 ) {
return temp.text;}
else {
return ", "+temp.text;}
});
}
r.text=obj;
r.data="var";
return r;
}
public  override  object @base( TemplateDefineContext context ){
var item = (new TemplateItem());
item.Template+="<";
foreach (var i in range(0,context.templateDefineItem().Length,1,true,false)){
if ( i>0 ) {
item.Template+=",";
if ( item.Contract.len()>0 ) {
item.Contract+=",";
}
}
var r = ((TemplateItem)(Visit(context.templateDefineItem(i))));
item.Template+=r.Template;
item.Contract+=r.Contract;
}
item.Template+=">";
return item;
}
public  override  object @base( TemplateDefineItemContext context ){
var item = (new TemplateItem());
if ( context.id().len()==1 ) {
var id1 = context.id(0).GetText();
item.Template=id1;
}
else {
var id1 = context.id(0).GetText();
item.Template=id1;
var id2 = context.id(1).GetText();
item.Contract=(new System.Text.StringBuilder(" where ").Append(id1).Append(":").Append(id2).Append("")).to_str();
}
return item;
}
public  override  object @base( TemplateCallContext context ){
var obj = "";
obj+="<";
foreach (var i in range(0,context.typeType().Length,1,true,false)){
if ( i>0 ) {
obj+=",";
}
var r = Visit(context.typeType(i));
obj+=r;
}
obj+=">";
return obj;
}
public  override  object @base( StringExpressionContext context ){
var text = (new System.Text.StringBuilder("(new System.Text.StringBuilder(").Append(context.TextLiteral().GetText()).Append(")")).to_str();
foreach (var item in context.stringExpressionElement()){
text+=Visit(item);
}
text+=").to_str()";
return (new Result(){data = Str,text = text});
}
public  override  object @base( StringExpressionElementContext context ){
var r = ((Result)(Visit(context.expression())));
var text = context.TextLiteral().GetText();
return (new System.Text.StringBuilder(".Append(").Append(r.text).Append(").Append(").Append(text).Append(")")).to_str();
}
public  override  object @base( DataStatementContext context ){
var r = (new Result());
if ( context.nilExpr()!=null ) {
r.data=Any;
r.text="null";
}
else if ( context.floatExpr()!=null ) {
r.data=F64;
r.text=((string)(Visit(context.floatExpr())));
}
else if ( context.integerExpr()!=null ) {
r.data=I32;
r.text=((string)(Visit(context.integerExpr())));
}
else if ( context.t.Type==TextLiteral ) {
r.data=Str;
r.text=context.TextLiteral().GetText();
}
else if ( context.t.Type==LiteParser.CharLiteral ) {
r.data=Chr;
r.text=context.CharLiteral().GetText();
}
else if ( context.t.Type==LiteParser.TrueLiteral ) {
r.data=Bool;
r.text=T;
}
else if ( context.t.Type==LiteParser.FalseLiteral ) {
r.data=Bool;
r.text=F;
}
return r;
}
public  override  object @base( FloatExprContext context ){
var number = "";
number+=Visit(context.integerExpr(0))+"."+Visit(context.integerExpr(1));
return number;
}
public  override  object @base( IntegerExprContext context ){
var number = "";
number+=context.NumberLiteral().GetText();
return number;
}
public  override  object @base( PlusMinusContext context ){
var r = (new Result());
var expr = ((Result)(Visit(context.expression())));
var op = Visit(context.add());
r.data=expr.data;
r.text=op+expr.text;
return r;
}
public  override  object @base( NegateContext context ){
var r = (new Result());
var expr = ((Result)(Visit(context.expression())));
r.data=expr.data;
r.text="!"+expr.text;
return r;
}
public  override  object @base( BitwiseNotExpressionContext context ){
var r = (new Result());
var expr = ((Result)(Visit(context.expression())));
r.data=expr.data;
r.text="~"+expr.text;
return r;
}
public  override  object @base( LinqContext context ){
var r = (new Result(){data = "var"});
r.text+=(new System.Text.StringBuilder("from ").Append(((Result)(Visit(context.expression(0)))).text).Append(" ")).to_str();
foreach (var item in context.linqItem()){
r.text+=(new System.Text.StringBuilder("").Append(Visit(item)).Append(" ")).to_str();
}
r.text+=(new System.Text.StringBuilder("").Append(context.k.Text).Append(" ").Append(((Result)(Visit(context.expression(1)))).text).Append("")).to_str();
return r;
}
public  override  object @base( LinqItemContext context ){
var obj = ((string)(Visit(context.linqKeyword())));
if ( context.expression()!=null ) {
obj+=(new System.Text.StringBuilder(" ").Append(((Result)(Visit(context.expression()))).text).Append("")).to_str();
}
return obj;
}
public  override  object @base( LinqKeywordContext context ){
return Visit(context.GetChild(0));
}
public  override  object @base( LinqHeadKeywordContext context ){
return context.k.Text;
}
public  override  object @base( LinqBodyKeywordContext context ){
return context.k.Text;
}
}
public partial class Compiler_static{
public static list<string> keywords = (new list<string>(){"abstract","as","base","bool","break","byte","case","catch","char","checked","class","const","continue","decimal","default","delegate","do","double","_","enum","event","explicit","extern","false","finally","fixed","float","for","foreach","goto","?","implicit","in","int","interface","internal","is","lock","long","namespace","new","null","object","operator","out","override","params","private","protected","public","readonly","ref","return","sbyte","sealed","short","sizeof","stackalloc","static","string","struct","switch","this","throw","true","try","typeof","uint","ulong","unchecked","unsafe","ushort","using","virtual","void","volatile","while"}) ;
}
}
