

function string_equals(%s1, %s2)
{
	if (%s1 !$= %s2)
	{
		error("ERROR: \"" @ %s1 @ "\" is not equal to \"" @ %s2 @ "\"");
		return 1;
	}
	return 0;
}

function testParseItemList()
{
	// exec("./storage.cs");
	%ct = 0;
	%s[%ct]				= "abcdefg" TAB 7;
	%sc[%ct++ - 1]		= "1" TAB "abcdefg";
	%s[%ct]				= "abcdefg" TAB 6;
	%sc[%ct++ - 1]		= "-1";
	%s[%ct]				= "123 abcdefg" TAB 6;
	%sc[%ct++ - 1]		= "-1";
	%s[%ct]				= "a b c d e f g   " TAB 13;
	%sc[%ct++ - 1]		= "1" TAB "a b c d e f g";
	%s[%ct]				= "a b c d e f g" TAB 12;
	%sc[%ct++ - 1]		= "2" TAB "a b c d e f" TAB "g";
	%s[%ct]				= "a b c d e f g" TAB 11;
	%sc[%ct++ - 1]		= "2" TAB "a b c d e f" TAB "g";
	%s[%ct]				= "a b c d e f g" TAB 10;
	%sc[%ct++ - 1]		= "2" TAB "a b c d e" TAB "f g";
	%s[%ct]				= "abc d e f g h ijk" TAB 5;
	%sc[%ct++ - 1]		= "3" TAB "abc d" TAB "e f g" TAB "h ijk";


	%curr = 0;
	while (%s[%curr] !$= "")
	{
		%error = string_equals(
			parseItemList(getField(%s[%curr], 0), getField(%s[%curr], 1)),
			%sc[%curr]);
		%curr++;
	}
}