<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="Group">
		<Group GroupNameid="{Id}" GroupName="{Name}"  />
	</xsl:template>
	<xsl:template match="GroupNames">
		<DocumentElement>
			<xsl:apply-templates />
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>