Public Class TwStockAccessor
    Private Shared mObj As New TwStockAccessor
    Public Shared ReadOnly Property Instance() As TwStockAccessor
        Get
            Return mObj
        End Get
    End Property

    Public Function Query(ByVal value As Date) As twStocks.Selection1DataTable
        Dim da As New twStocksTableAdapters.Selection1TableAdapter
        Return da.GetData(value)
    End Function

    ''' <summary>
    ''' 取得停損條件中文名稱
    ''' </summary>
    Public Function GetRuleName(ByVal rule As SelectionRule, ByVal direct As Direct, ByRef tipTitle As String, ByRef tipText As String) As String
        tipTitle = ""
        tipText = ""
        Select Case direct
            Case direct.Rise
                Select Case rule
                    Case SelectionRule.ThroughInterval
                        'ruleTip = "<img title='<h3>突破整理區間</h3><p>區間整理是指股價在一段時間內波動幅度小，無明顯的上漲或下降趨勢，為未來的方向準備突破的動能。當股價突破（或跌破）了整理區間，通常表示動能爆發出現方向，將會是可攻擊的目標！</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px"" />"
                        tipTitle = "突破整理區間"
                        tipText = "區間整理是指股價在一段時間內波動幅度小，無明顯的上漲或下降趨勢，為未來的方向準備突破的動能。當股價突破（或跌破）了整理區間，通常表示動能爆發出現方向，將會是可攻擊的目標！"
                        Return "多：突破整理區間"
                    Case SelectionRule.KDCross
                        'ruleTip = "<img  title='<h3>KD黃金交叉</h3><p>KD指標針對指數而言，通常在80以上被視為超買區，在20以下則視為超賣區，針對個股則需考量該個股是否有短線超漲與超跌的特性。由於D值較K值平緩，因此當K值在超賣區向上穿越D值時，表示趨勢發生改變，為買進訊號。(即一般所稱KD指標低檔交叉向上時為買進訊號，亦可稱為黃金交叉)。而當K值在超買區向下跌破D值時則為賣出訊號(亦可稱為死亡交叉)。</p><p><a class=""imgtitle"" href=""/53697/150"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "KD黃金交叉"
                        tipText = "KD指標針對指數而言，通常在80以上被視為超買區，在20以下則視為超賣區，針對個股則需考量該個股是否有短線超漲與超跌的特性。由於D值較K值平緩，因此當K值在超賣區向上穿越D值時，表示趨勢發生改變，為買進訊號。(即一般所稱KD指標低檔交叉向上時為買進訊號，亦可稱為黃金交叉)。而當K值在超買區向下跌破D值時則為賣出訊號(亦可稱為死亡交叉)。</p><p><a class=""imgtitle"" href=""/53697/150"" target=""_blank"">更多說明</a>"
                        Return "多：KD黃金交叉"
                    Case SelectionRule.LargeVolume
                        'ruleTip = "<img title='<h3>爆量長紅</h3><p>同時出現爆量+長紅K棒的股票&#13;長紅條件為:大漲半支停板以上<br/>爆量:成交量超過五日均量(2倍以上)<br/>參考文章<br/><a class=""imgtitle"" href=""/madchu/667"" target=""_blank"">輕鬆選出飆股的方法</a></p><p><a class=""imgtitle"" href=""/53697/142"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "爆量長紅"
                        tipText = "同時出現爆量+長紅K棒的股票&#13;長紅條件為:大漲半支停板以上<br/>爆量:成交量超過五日均量(2倍以上)<br/>參考文章<br/><a class=""imgtitle"" href=""/madchu/667"" target=""_blank"">輕鬆選出飆股的方法</a></p><p><a class=""imgtitle"" href=""/53697/142"" target=""_blank"">更多說明</a>"
                        Return "多：爆量長紅"
                    Case SelectionRule.Through10Mean
                        'ruleTip = "<img  title='<h3>突破十日均線</h3><p>找出股價突破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "突破十日均線"
                        tipText = "找出股價突破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a>"
                        Return "多：突破十日均線"
                    Case SelectionRule.Through20Mean
                        'ruleTip = "<img  title='<h3>突破月線</h3><p>找出股價突破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "突破月線"
                        tipText = "找出股價突破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a>"
                        Return "多：突破月線"
                    Case SelectionRule.Through50Mean
                        'ruleTip = "<img  title='<h3>日均線</h3><p>找出股價突破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "日均線"
                        tipText = "找出股價突破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a>"
                        Return "多：突破50日均線"
                    Case SelectionRule.Through5Mean
                        'ruleTip = "<img  title='<h3>突破周線</h3><p>找出股價突破均線的股票，短期均線是用來找買賣訊號。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "突破周線"
                        tipText = "找出股價突破均線的股票，短期均線是用來找買賣訊號。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a>"
                        Return "多：突破周線"
                    Case SelectionRule.Through60Mean
                        'ruleTip = "<img  title='<h3>突破季線</h3><p>找出股價突破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "突破季線"
                        tipText = "找出股價突破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。</p><p><a class=""imgtitle"" href=""/53697/147"" target=""_blank"">更多說明</a>"
                        Return "多：突破季線"
                    Case SelectionRule.Swallow
                        'ruleTip = "<img  title='<h3>多頭吞噬</h3><p>這是一種空頭轉多頭的轉折，條件有三個：&#13;1.下跌勢中&#13;2.跳空下跌導致開盤價比前一個交易日的最低點還要低&#13;3.收盤上漲比前一個交易日的最高點還要高</p><p><a class=""imgtitle"" href=""/53697/153"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "多頭吞噬"
                        tipText = "這是一種空頭轉多頭的轉折，條件有三個：&#13;1.下跌勢中&#13;2.跳空下跌導致開盤價比前一個交易日的最低點還要低&#13;3.收盤上漲比前一個交易日的最高點還要高</p><p><a class=""imgtitle"" href=""/53697/153"" target=""_blank"">更多說明</a>"
                        Return "多：多頭吞噬"
                    Case SelectionRule.Cover
                        'ruleTip = "<img  title='<h3>貫穿</h3><p>跌勢中隔天跳空開低在黑K低點下面，收盤漲到前一天的黑K棒的一半以上</p><p><a class=""imgtitle"" href=""/53697/154"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "貫穿"
                        tipText = "跌勢中隔天跳空開低在黑K低點下面，收盤漲到前一天的黑K棒的一半以上</p><p><a class=""imgtitle"" href=""/53697/154"" target=""_blank"">更多說明</a>"
                        Return "多：貫穿"
                    Case SelectionRule.MeanCross
                        'ruleTip = "<img  title='<h3>均線黃金交叉</h3><p>短週期均線往上穿越長週期均線，稱為「黃金交叉」。通常會帶來一段多頭行情。一般來講，出現黃金交叉為買進信號，後勢看漲。</p><p><a class=""imgtitle"" href=""/53697/148"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "均線黃金交叉"
                        tipText = "短週期均線往上穿越長週期均線，稱為「黃金交叉」。通常會帶來一段多頭行情。一般來講，出現黃金交叉為買進信號，後勢看漲。</p><p><a class=""imgtitle"" href=""/53697/148"" target=""_blank"">更多說明</a>"
                        Return "多：5日與20日均線黃金交叉"
                    Case SelectionRule.Mean510Cross
                        'ruleTip = "<img  title='<h3>均線黃金交叉</h3><p>短週期均線往上穿越長週期均線，稱為「黃金交叉」。通常會帶來一段多頭行情。一般來講，出現黃金交叉為買進信號，後勢看漲。</p><p><a class=""imgtitle"" href=""/53697/148"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "均線黃金交叉"
                        tipText = "短週期均線往上穿越長週期均線，稱為「黃金交叉」。通常會帶來一段多頭行情。一般來講，出現黃金交叉為買進信號，後勢看漲。</p><p><a class=""imgtitle"" href=""/53697/148"" target=""_blank"">更多說明</a>"
                        Return "多：5日與10日均線黃金交叉"
                    Case SelectionRule.News
                        'ruleTip = "<img  title='<h3>利多消息</h3><p>是指能刺激股價上揚的消息，例如：公司接獲大訂單、獲利超前、產業前景看好、投資法令放寬等等。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "利多消息"
                        tipText = "是指能刺激股價上揚的消息，例如：公司接獲大訂單、獲利超前、產業前景看好、投資法令放寬等等。"
                        Return "多：利多消息"
                    Case SelectionRule.LargeVolumeContinue
                        'ruleTip = "<img  title='<h3>連續爆量長紅</h3><p>爆量(成交量超過五日均量2倍以上)長紅而且第二天連續攻擊<br/>參考文章<br/><a class=""imgtitle"" href=""/madchu/667"" target=""_blank"">輕鬆選出飆股的方法</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "連續爆量長紅"
                        tipText = "爆量(成交量超過五日均量2倍以上)長紅而且第二天連續攻擊<br/>參考文章<br/><a class=""imgtitle"" href=""/madchu/667"" target=""_blank"">輕鬆選出飆股的方法</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a>"
                        Return "多：連續爆量長紅"
                    Case SelectionRule.Combine3
                        'ruleTip = "<img  title='<h3>均線3合一</h3><p>當5日,10日,20日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "均線3合一"
                        tipText = "當5日,10日,20日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a>"
                        Return "多：均線3合一"
                    Case SelectionRule.Combine4
                        'ruleTip = "<img  title='<h3>均線4合一</h3><p>當5日,10日,20日,60日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "均線4合一"
                        tipText = "當5日,10日,20日,60日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a>"
                        Return "多：均線4合一"
                    Case SelectionRule.ThroughHighLow
                        Return "多：過前高"
                End Select
            Case direct.Fall
                Select Case rule
                    Case SelectionRule.ThroughInterval
                        'ruleTip = "<img title='<h3>跌破整理區間</h3><p>區間整理是指股價在一段時間內波動幅度小，無明顯的上漲或下降趨勢，為未來的方向準備突破的動能。當股價突破（或跌破）了整理區間，通常表示動能爆發出現方向，將會是可攻擊的目標！</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px"" />"
                        tipTitle = "跌破整理區間"
                        tipText = "區間整理是指股價在一段時間內波動幅度小，無明顯的上漲或下降趨勢，為未來的方向準備突破的動能。當股價突破（或跌破）了整理區間，通常表示動能爆發出現方向，將會是可攻擊的目標！"
                        Return "空：跌破整理區間"
                    Case SelectionRule.KDCross
                        'ruleTip = "<img  title='<h3>KD死亡交叉</h3><p>KD指標針對指數而言，通常在80以上被視為超買區，在20以下則視為超賣區，針對個股則需考量該個股是否有短線超漲與超跌的特性。由於D值較K值平緩，因此當K值在超賣區向上穿越D值時，表示趨勢發生改變，為買進訊號。(即一般所稱KD指標低檔交叉向上時為買進訊號，亦可稱為黃金交叉)。而當K值在超買區向下跌破D值時則為賣出訊號(亦可稱為死亡交叉)。</p><p><a class=""imgtitle"" href=""/53697/150"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "KD死亡交叉"
                        tipText = "KD指標針對指數而言，通常在80以上被視為超買區，在20以下則視為超賣區，針對個股則需考量該個股是否有短線超漲與超跌的特性。由於D值較K值平緩，因此當K值在超賣區向上穿越D值時，表示趨勢發生改變，為買進訊號。(即一般所稱KD指標低檔交叉向上時為買進訊號，亦可稱為黃金交叉)。而當K值在超買區向下跌破D值時則為賣出訊號(亦可稱為死亡交叉)。</p><p><a class=""imgtitle"" href=""/53697/150"" target=""_blank"">更多說明</a>"
                        Return "空：KD死亡交叉"
                    Case SelectionRule.LargeVolume
                        'ruleTip = "<img title='<h3>爆量長黑</h3><p>同時出現爆量+長黑K棒的股票&#13;長黑條件為:大跌半支停板以上<br/>爆量:成交量超過五日均量(2倍以上)</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "爆量長黑"
                        tipText = "同時出現爆量+長黑K棒的股票&#13;長黑條件為:大跌半支停板以上<br/>爆量:成交量超過五日均量(2倍以上)"
                        Return "空：爆量長黑"
                    Case SelectionRule.Through10Mean
                        'ruleTip = "<img  title='<h3>跌破十日均線</h3><p>找出股價跌破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "跌破十日均線"
                        tipText = "找出股價跌破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。"
                        Return "空：跌破十日均線"
                    Case SelectionRule.Through20Mean
                        'ruleTip = "<img  title='<h3>跌破月線</h3><p>找出股價跌破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "跌破月線"
                        tipText = "找出股價跌破均線的股票，短期均線是用來找買賣訊號，通常使用10日均線與月線。"
                        Return "空：跌破月線"
                    Case SelectionRule.Through50Mean
                        'ruleTip = "<img  title='<h3>跌破50日均線</h3><p>找出股價跌破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "跌破50日均線"
                        tipText = "找出股價跌破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。"
                        Return "空：跌破50日均線"
                    Case SelectionRule.Through5Mean
                        'ruleTip = "<img  title='<h3>跌破周線</h3><p>找出股價跌破均線的股票，短期均線是用來找買賣訊號。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "跌破周線"
                        tipText = "找出股價跌破均線的股票，短期均線是用來找買賣訊號。"
                        Return "空：跌破周線"
                    Case SelectionRule.Through60Mean
                        'ruleTip = "<img  title='<h3>跌破季線</h3><p>找出股價跌破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "跌破季線"
                        tipText = "找出股價跌破均線的股票，中長期均線可研判趨勢方向，使用季線、半年線、年線。"
                        Return "空：跌破季線"
                    Case SelectionRule.Swallow
                        'ruleTip = "<img  title='<h3>空頭吞噬</h3><p>空頭吞噬是一種多頭轉空頭的轉折線型，條件有三個：&#13;1.在上漲勢中&#13;2.跳空開高比前一天高點要高&#13;3.收盤殺低比前一天低點要低</p><p><a class=""imgtitle"" href=""/53697/153"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "空頭吞噬"
                        tipText = "空頭吞噬是一種多頭轉空頭的轉折線型，條件有三個：&#13;1.在上漲勢中&#13;2.跳空開高比前一天高點要高&#13;3.收盤殺低比前一天低點要低</p><p><a class=""imgtitle"" href=""/53697/153"" target=""_blank"">更多說明</a>"
                        Return "空：空頭吞噬"
                    Case SelectionRule.Cover
                        'ruleTip = "<img  title='<h3>烏雲罩頂</h3><p>漲勢中出現紅K棒，隔天跳空開高之後下殺，尾盤跌到前一根紅K棒的實體K的一半以下，為一種多翻空的反轉型態。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "烏雲罩頂"
                        tipText = "漲勢中出現紅K棒，隔天跳空開高之後下殺，尾盤跌到前一根紅K棒的實體K的一半以下，為一種多翻空的反轉型態。"
                        Return "空：烏雲罩頂"
                    Case SelectionRule.MeanCross
                        'ruleTip = "<img  title='<h3>5日與20日均線死亡交叉</h3><p>短週期均線往下跌破長週期均線，稱為「死亡交叉」。通常可能會有一段跌勢即將發生。一般來講，出現死亡交叉為賣出信號，後勢看跌。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "5日與20日均線死亡交叉"
                        tipText = "短週期均線往下跌破長週期均線，稱為「死亡交叉」。通常可能會有一段跌勢即將發生。一般來講，出現死亡交叉為賣出信號，後勢看跌。"
                        Return "空：5日與20日均線死亡交叉"
                    Case SelectionRule.Mean510Cross
                        'ruleTip = "<img  title='<h3>5日與10日均線死亡交叉</h3><p>短週期均線往下跌破長週期均線，稱為「死亡交叉」。通常可能會有一段跌勢即將發生。一般來講，出現死亡交叉為賣出信號，後勢看跌。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "5日與10日均線死亡交叉"
                        tipText = "短週期均線往下跌破長週期均線，稱為「死亡交叉」。通常可能會有一段跌勢即將發生。一般來講，出現死亡交叉為賣出信號，後勢看跌。"
                        Return "空：5日與10日均線死亡交叉"
                    Case SelectionRule.News
                        'ruleTip = "<img  title='<h3>利空消息</h3><p>是指會造成股價下跌的消息，例如：公司裁員、公司產品有瑕疵，被退貨、產業進入衰退期、調降投資評等、營收獲利表現不佳等等。</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "利空消息"
                        tipText = "是指會造成股價下跌的消息，例如：公司裁員、公司產品有瑕疵，被退貨、產業進入衰退期、調降投資評等、營收獲利表現不佳等等。</p>"
                        Return "空：利空消息"
                    Case SelectionRule.LargeVolumeContinue
                        'ruleTip = "<img  title='<h3>連續爆量長黑</h3><p>爆量(成交量超過五日均量2倍以上)長黑而且第二天連續攻擊</p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "連續爆量長黑"
                        tipText = "爆量(成交量超過五日均量2倍以上)長黑而且第二天連續攻擊</p>"
                        Return "空：連續爆量長黑"
                    Case SelectionRule.Combine3
                        'ruleTip = "<img  title='<h3>均線3合一</h3><p>當5日,10日,20日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "均線3合一"
                        tipText = "當5日,10日,20日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a>"
                        Return "空：均線3合一"
                    Case SelectionRule.Combine4
                        'ruleTip = "<img  title='<h3>均線4合一</h3><p>當5日,10日,20日,60日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""m/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a></p>' style=""opacity:0.6;"" src=""/image/icons/query1.png"" width=""14px""/>"
                        tipTitle = "均線4合一"
                        tipText = "當5日,10日,20日,60日均線糾結在很小的範圍時，短期、中期、長期投資人的持股成本都非常接近，糾結處往往都是起漲或起跌點。建議搭配長紅爆量使用。<br/>參考文章<br/><a  class=""imgtitle"" href=""m/cto/16"" target=""_blank"">短線飆派軍團長的必漲選股密技</a><br/><a href=""/KPMG/587"" target=""_blank"">技術性分析的飆速感-均線合一爆發力</a></p><p><a class=""imgtitle"" href=""/53697/144"" target=""_blank"">更多說明</a>"
                        Return "空：均線4合一"
                    Case SelectionRule.ThroughHighLow
                        Return "空：破前低"
                End Select
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' 取得停損條件中文名稱 - 簡短
    ''' </summary>
    Public Function GetRuleName_short(ByVal rule As SelectionRule, ByVal direct As Direct) As String
        Select Case direct
            Case direct.Rise
                Select Case rule
                    Case SelectionRule.ThroughInterval
                        Return "突破整理區間"
                    Case SelectionRule.KDCross
                        Return "KD黃金交叉"
                    Case SelectionRule.LargeVolume
                        Return "爆量長紅"
                    Case SelectionRule.Through10Mean
                        Return "突破10日均線"
                    Case SelectionRule.Through20Mean
                        Return "突破月線"
                    Case SelectionRule.Through50Mean
                        Return "突破50日均線"
                    Case SelectionRule.Through5Mean
                        Return "突破周線"
                    Case SelectionRule.Through60Mean
                        Return "突破季線"
                    Case SelectionRule.Swallow
                        Return "多頭吞噬"
                    Case SelectionRule.Cover
                        Return "貫穿"
                    Case SelectionRule.MeanCross
                        Return "5與20日均線黃金交叉"
                    Case SelectionRule.Mean510Cross
                        Return "5與10日均線黃金交叉"
                    Case SelectionRule.News
                        Return "利多消息"
                    Case SelectionRule.LargeVolumeContinue
                        Return "連續爆量長紅"
                    Case SelectionRule.Combine3
                        Return "均線3合一"
                    Case SelectionRule.Combine4
                        Return "均線4合一"
                    Case SelectionRule.ThroughHighLow
                        Return "過前高"
                End Select
            Case direct.Fall
                Select Case rule
                    Case SelectionRule.ThroughInterval
                        Return "跌破整理區間"
                    Case SelectionRule.KDCross
                        Return "KD死亡交叉"
                    Case SelectionRule.LargeVolume
                        Return "爆量長黑"
                    Case SelectionRule.Through10Mean
                        Return "跌破十日均線"
                    Case SelectionRule.Through20Mean
                        Return "跌破月線"
                    Case SelectionRule.Through50Mean
                        Return "跌破50日均線"
                    Case SelectionRule.Through5Mean
                        Return "跌破周線"
                    Case SelectionRule.Through60Mean
                        Return "跌破季線"
                    Case SelectionRule.Swallow
                        Return "空頭吞噬"
                    Case SelectionRule.Cover
                        Return "烏雲罩頂"
                    Case SelectionRule.MeanCross
                        Return "5與20日均線死亡交叉"
                    Case SelectionRule.Mean510Cross
                        Return "5與10日均線死亡交叉"
                    Case SelectionRule.News
                        Return "利空消息"
                    Case SelectionRule.LargeVolumeContinue
                        Return "連續爆量長黑"
                    Case SelectionRule.Combine3
                        Return "均線3合一"
                    Case SelectionRule.Combine4
                        Return "均線4合一"
                    Case SelectionRule.ThroughHighLow
                        Return "破前低"
                End Select
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' 取得漲停價
    ''' </summary>
    Public Function GetUpLimitPrice(ByVal value As Double) As Double

        Dim price As Double = value * (mPriceLimit / 100) + value

        Dim basicRangePrice As Double = GetBasicRangePrice(price)
        If basicRangePrice = 0 Then Return Val(Format(price, "0.00"))

        Return Val(Format(Math.Floor(price / basicRangePrice) * basicRangePrice, "0.00"))
    End Function

    ''' <summary>
    ''' 漲跌幅限制
    ''' </summary>
    Private mPriceLimit As Integer = 7

    ''' <summary>
    ''' 取得跌停價
    ''' </summary>
    Public Function GetDownLimitPrice(ByVal value As Double) As Double
        Dim price As Double = value * (-mPriceLimit / 100) + value

        Dim basicRangePrice As Double = GetBasicRangePrice(price)
        If basicRangePrice = 0 Then Return Val(Format(price, "0.00"))

        Return Val(Format(Math.Ceiling(price / basicRangePrice) * basicRangePrice, "0.00"))
    End Function

    ''' <summary>
    ''' 取得基本跳動值
    ''' </summary>
    Public Function GetBasicRangePrice(ByVal value As Double) As Double
        If value > 1000 Then
            Return 5
        ElseIf value > 500 Then
            Return 1
        ElseIf value > 100 Then
            Return 0.5
        ElseIf value > 50 Then
            Return 0.1
        ElseIf value > 10 Then
            Return 0.05
        ElseIf value > 0 Then
            Return 0.01
        Else
            Return 0.01
        End If
    End Function

    ''' <summary>
    ''' 取得會員台股成交記錄
    ''' </summary>
    Public Function GetTradeRecords(ByVal memberNo As Integer, ByVal stockNo As Integer, ByVal startDate As Date, ByVal endDate As Date) As twStocks.GetTradeRecordDataTable
        Dim da As New twStocksTableAdapters.GetTradeRecordTableAdapter
        Return da.GetData(memberNo, stockNo, startDate, endDate)
    End Function

    Private mName As New Generic.Dictionary(Of String, String)
    ''' <summary>
    ''' 取得股票名稱
    ''' </summary>
    Public Function GetName(ByVal stockNo As String) As String
        If mName.ContainsKey(stockNo.Trim) Then
            Return mName(stockNo)
        End If

        mName.Clear()
        Dim da As New twStocksTableAdapters.StockTableAdapter
        For Each stock As twStocks.StockRow In da.GetData.Rows
            With stock
                If Not mName.ContainsKey(.StockNo.Trim) Then
                    mName.Add(.StockNo.Trim, .Name.Trim)
                End If
            End With
        Next

        If mName.ContainsKey(stockNo.Trim) Then
            Return mName(stockNo)
        End If

        Return ""
    End Function


    Public Function GetRealTimeChart(ByVal stockNo As String, Optional ByVal width As Integer = 550, Optional ByVal height As Integer = 250) As String
        Dim chart As New RealTimeChart
        With chart
            .Title = stockNo.Replace("&", "") + " " + GetName(stockNo)
            .TitleColor = "0000FF"

            Dim da As New twStocksTableAdapters.RealTimePriceTableAdapter
            Dim price As New Generic.List(Of Double)
            Dim time As New Generic.List(Of Date)
            Dim volume As New Generic.List(Of Integer)
            Dim hasVoume As Boolean
            Dim isAdd As Boolean

            Dim maxTime As New Date(1911, 1, 1)

            For Each row As twStocks.RealTimePriceRow In da.GetData(stockNo)
                If Not isAdd Then .Open = row.Deal - row.Change
                price.Add(row.Deal)
                time.Add(row.Time)
                volume.Add(row.Volume)

                If maxTime.Year = 1911 Then
                    maxTime = row.Time
                Else
                    If row.Time.Subtract(maxTime).TotalHours > 0 Then
                        maxTime = row.Time
                    End If
                End If
            Next

            If price.Count <= 2 Then Return ""

            'Dim minTime As New Date(3000, 1, 1)

            'Dim newPrices As New Generic.List(Of Double)
            'Dim newTimes As New Generic.List(Of Date)
            'Dim newVolumes As New Generic.List(Of Integer)

            'For index As Integer = 0 To time.Count - 1

            '    If maxTime.Subtract(time(index)).TotalHours < 20 Then

            '        If minTime.Year = 3000 Then
            '            minTime = time(index)
            '        Else
            '            If minTime.Subtract(newTimes(index)).TotalHours > 0 Then
            '                minTime = newTimes(index)
            '            End If
            '        End If

            '        newPrices.Add(price(index))
            '        newTimes.Add(time(index))
            '        newVolumes.Add(volume(index))

            '    End If

            'Next

            'price = newPrices
            'time = newTimes
            'volume = newVolumes

            '.StartTime = minTime
            '.EndTime = maxTime

            Dim newStartTime As Date = Now.AddMinutes(-70)

            If newStartTime.Hour < 9 Then
                .StartTime = New Date(Now.Year, Now.Month, Now.Day, 9, 0, 0)
            Else
                .StartTime = New Date(Now.Year, Now.Month, Now.Day, newStartTime.Hour, newStartTime.Minute, 0)
            End If
            .EndTime = New Date(Now.Year, Now.Month, Now.Day, 13, 30, 0)

            If Not hasVoume Then volume.Clear()

            .SetData(price, time, volume, "2222FF")
            .Width = width
            .Height = height

            'If Val(stockNo) > 1000 Then
            '    .Max = GetUpLimitPrice(.Open)
            '    .Min = GetDownLimitPrice(.Open)
            'Else
            .AutoSetMaxMin()
            .AutoSetTimeAxis()
            'End If
            .SetUpdateTimeIsLastData()

        End With

        Return chart.GetChart
    End Function

    'Public Function GetRealTimeChartByGlobal(ByVal stockNo As String, Optional ByVal width As Integer = 550, Optional ByVal height As Integer = 250) As String
    '    Dim chart As New RealTimeChart
    '    With chart
    '        .Title = stockNo.Replace("&", "") + " " + GetName(stockNo)
    '        .TitleColor = "0000FF"

    '        Dim price As New Generic.List(Of Double)
    '        Dim time As New Generic.List(Of Date)
    '        Dim volume As New Generic.List(Of Integer)
    '        Dim hasVoume As Boolean
    '        Dim isAdd As Boolean

    '        Dim maxTime As New Date(1911, 1, 1)

    '        Dim minTime As New Date(2200, 1, 1)

    '        Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
    '        connection.Open()
    '        Try
    '            Dim cmd As String = ""

    '            cmd = "SELECT * FROM RealTimePrice Where StockNo='" & stockNo & "' order by Time desc"

    '            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
    '            command.ExecuteNonQuery()
    '            Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
    '            Dim dataSet As New Data.DataSet
    '            adapter.Fill(dataSet)

    '            For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
    '                If Not isAdd Then .Open = row("Deal") - row("Change")
    '                price.Add(row("Deal"))
    '                time.Add(row("Time"))
    '                volume.Add(row("Volume"))

    '                If maxTime.Year = 1911 Then
    '                    maxTime = CDate(row("Time"))
    '                Else
    '                    If CDate(row("Time")).Subtract(maxTime).TotalHours > 0 Then
    '                        maxTime = CDate(row("Time"))
    '                    End If
    '                End If
    '            Next

    '            For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
    '                If minTime.Year = 2200 Then
    '                    minTime = CDate(row("Time"))
    '                Else
    '                    If minTime.Subtract(CDate(row("Time"))).TotalHours > 0 Then
    '                        minTime = CDate(row("Time"))
    '                    End If
    '                End If
    '            Next
    '        Catch ex As Exception

    '        End Try

    '        connection.Close()
    '        connection.Dispose()

    '        If price.Count <= 2 Then Return ""

    '        .StartTime = minTime
    '        .EndTime = maxTime

    '        If Not hasVoume Then volume.Clear()

    '        .SetData(price, time, volume, "2222FF")
    '        .Width = width
    '        .Height = height

    '        'If Val(stockNo) > 1000 Then
    '        '    .Max = GetUpLimitPrice(.Open)
    '        '    .Min = GetDownLimitPrice(.Open)
    '        'Else
    '        .AutoSetMaxMin()
    '        .AutoSetTimeAxis()
    '        'End If
    '        .SetUpdateTimeIsLastData()

    '    End With

    '    Return chart.GetChart
    'End Function

    Public Function GetRealTimeChartByGlobal(ByVal stockNo As String, Optional ByVal width As Integer = 550, Optional ByVal height As Integer = 250) As String
        Dim chart As New RealTimeChartV2
        With chart
            '.Title = stockNo.Replace("&", "") + " " + TwStockAccessor.Instance.GetName(stockNo)
            '.TitleColor = "0000FF"

            'Dim da As New twStocksTableAdapters.RealTimePriceTableAdapter

            Dim connection As New System.Data.SqlClient.SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings("twStocksConnectionString").ConnectionString)
            connection.Open()
            Dim cmd As String = ""

            cmd = "SELECT * FROM RealTimePrice Where StockNo='" & stockNo & "' order by Time"

            Dim command As New System.Data.SqlClient.SqlCommand(cmd, connection)
            command.ExecuteNonQuery()
            Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
            Dim dataSet As New Data.DataSet
            adapter.Fill(dataSet)

            Dim price As New Generic.List(Of Double)
            Dim time As New Generic.List(Of Date)
            Dim volume As New Generic.List(Of Integer)
            Dim hasVoume As Boolean

            Dim maxTime As New Date(1911, 1, 1)

            'For Each row As twStocks.RealTimePriceRow In da.GetData(stockNo)
            For Each row As Data.DataRow In dataSet.Tables.Item(0).Rows
                .Open = row("Deal") - row("Change")

                price.Add(row("Deal"))
                time.Add(CDate(row("Time")))
                volume.Add(row("Volume"))

                If maxTime.Year = 1911 Then
                    maxTime = CDate(row("Time"))
                Else
                    If CDate(row("Time")).Subtract(maxTime).TotalHours > 0 Then
                        maxTime = CDate(row("Time"))
                    End If
                End If
            Next

            connection.Close()
            connection.Dispose()

            If price.Count <= 2 Then Return ""

            Dim minTime As New Date(3000, 1, 1)

            Dim newPrices As New Generic.List(Of Double)
            Dim newTimes As New Generic.List(Of Date)
            Dim newVolumes As New Generic.List(Of Integer)

            For index As Integer = time.Count - 1 To 0 Step -1

                If maxTime.Subtract(time(index)).TotalHours < 11 AndAlso maxTime.Day = time(index).Day Then
                    If minTime.Year = 3000 Then
                        minTime = time(index)
                    Else
                        If minTime.Subtract(time(index)).TotalHours > 0 Then
                            minTime = time(index)
                        End If
                    End If

                    newPrices.Add(price(index))
                    newTimes.Add(time(index))
                    newVolumes.Add(volume(index))
                End If

            Next

            newPrices.Reverse()
            newTimes.Reverse()
            newVolumes.Reverse()

            price = newPrices
            time = newTimes
            volume = newVolumes

            'refMaxTime = time(0).ToString + "######" + time(time.Count - 1).ToString

            .StartTime = minTime
            .EndTime = maxTime

            'If MemberDataAccessor.Instance.GetMemberNo = "736" Then
            '    Response.Redirect("http://tw.yyy.com/?id=" + price.Count.ToString)
            'End If

            If Not hasVoume Then volume.Clear()
            .SetData(price, time, volume, "2222FF")
            .Width = width
            .Height = height

            If Val(stockNo) > 1000 Then
                .Max = TwStockAccessor.Instance.GetUpLimitPrice(.Open)
                .Min = TwStockAccessor.Instance.GetDownLimitPrice(.Open)
            Else
                .AutoSetMaxMin()
                .AutoSetTimeAxis()
            End If
            .SetUpdateTimeIsLastData()

        End With
        Return chart.GetChart
    End Function
End Class

Public Class RealTimeChartV2

#Region "title"
    Private mTitle As String = ""
    'Public Property Title() As String
    '    Get
    '        Return mTitle
    '    End Get
    '    Set(ByVal value As String)
    '        mTitle = value
    '    End Set
    'End Property

    Private mTitleColor As String = "0000FF"
    Public Property TitleColor() As String
        Get
            Return mTitleColor
        End Get
        Set(ByVal value As String)
            mTitleColor = value
        End Set
    End Property

    Private mTitleSize As Integer = 13
    Public Property TitleSize() As Integer
        Get
            Return mTitleSize
        End Get
        Set(ByVal value As Integer)
            mTitleSize = value
        End Set
    End Property

    Private mUpdateTime As String = ""
    Public Property UpdateTime() As String
        Get
            Return mUpdateTime
        End Get
        Set(ByVal value As String)
            mUpdateTime = value
        End Set
    End Property

    Private Function GetTitleScript() As String
        If mTitle.Trim = "" Then Return ""
        Dim script As String = "&chtt=" + mTitle
        If mUpdateTime.TrimEnd <> "" Then
            script = script + " " + mUpdateTime.Trim
        End If
        script = script + "&chts=" + mTitleColor + "," + mTitleSize.ToString
        Return script
    End Function

#End Region

    Private mWidth As Integer = 600
    Public Property Width() As Integer
        Get
            Return mWidth
        End Get
        Set(ByVal value As Integer)
            mWidth = value
        End Set
    End Property

    Private mHeight As Integer = 250
    Public Property Height() As Integer
        Get
            Return mHeight
        End Get
        Set(ByVal value As Integer)
            mHeight = value
        End Set
    End Property

    Private Function GetSize() As String
        Dim script As String = "&chs=" + mWidth.ToString + "x" + mHeight.ToString
        Return script
    End Function

    Private mLineWidth As Double = 1
    Public Property LineWidth() As Double
        Get
            Return mLineWidth
        End Get
        Set(ByVal value As Double)
            mLineWidth = value
        End Set
    End Property

#Region "Line"

    Private mPrices As New Generic.List(Of Double)
    Private mTimes As New Generic.List(Of Date)
    Private mVolume As New Generic.List(Of Integer)

    Private mStartTime As Date
    Public Property StartTime() As Date
        Get
            Return mStartTime
        End Get
        Set(ByVal value As Date)
            mStartTime = value
        End Set
    End Property

    Private mEndTime As Date
    Public Property EndTime() As Date
        Get
            Return mEndTime
        End Get
        Set(ByVal value As Date)
            mEndTime = value
        End Set
    End Property

    Private mMax As Double
    Public Property Max() As Double
        Get
            Return mMax
        End Get
        Set(ByVal value As Double)
            mMax = value
        End Set
    End Property

    Private mMin As Double = 9999999
    Public Property Min() As Double
        Get
            Return mMin
        End Get
        Set(ByVal value As Double)
            mMin = value
        End Set
    End Property

    Private mOpen As Double = 0
    Public Property Open() As Double
        Get
            Return mOpen
        End Get
        Set(ByVal value As Double)
            mOpen = value
        End Set
    End Property

    Private mColor As String = "000000"
    Private mhasVolume As Boolean

    ''' <summary>
    ''' 設定股價資料數据
    ''' </summary>
    Public Sub SetData(ByVal prices As Generic.List(Of Double), _
                                  ByVal times As Generic.List(Of Date), _
                                  ByVal volume As Generic.List(Of Integer), _
                                  ByVal color As String)
        Me.mPrices.Clear()
        Me.mTimes.Clear()
        Me.mVolume.Clear()
        Me.mPrices.AddRange(prices)
        Me.mTimes.AddRange(times)
        Me.mVolume.AddRange(volume)
        mColor = color
        If mVolume.Count > 0 Then mhasVolume = True
    End Sub

    Public Sub SetUpdateTimeIsLastData()
        If mTimes.Count = 0 Then Exit Sub
        mUpdateTime = Format(mTimes(mTimes.Count - 1), "(MM/dd HH:mm)")
    End Sub

    Private mIsMaxMinAuto As Boolean
    Public Sub AutoSetMaxMin()
        If mPrices.Count = 0 Then Exit Sub
        mMax = 0
        mMin = 99999999
        For Each value As Double In mPrices
            mMax = Math.Max(value, mMax)
            mMin = Math.Min(value, mMin)
        Next

        If (mMax - mOpen) > (mOpen - mMin) Then
            mMin = mOpen - (mMax - mOpen)
        Else
            mMax = mOpen + (mOpen - mMin)
        End If

        mIsMaxMinAuto = True
    End Sub

    Private mIsTimeAxisAuto As Boolean
    Public Sub AutoSetTimeAxis()
        If mTimes.Count = 0 Then Exit Sub
        Dim startValue As Integer = Math.Floor(mTimes(0).Minute / 15)
        With mTimes(0)
            mStartTime = .AddMinutes(-.Minute + 15 * startValue)
        End With

        Dim endValue As Integer = Math.Ceiling(mTimes(mTimes.Count - 1).Minute / 15)
        With mTimes(mTimes.Count - 1)
            If endValue > 3 Then
                mEndTime = .AddHours(1).AddMinutes(-.Minute)
            Else
                mEndTime = .AddMinutes(-.Minute + 15 * endValue)
            End If
        End With
    End Sub

    Private Function GetLineScript() As String
        Dim script As String = "&chd=t:"
        Dim xRange As Double = mMax - mMin
        Dim timeRange As TimeSpan = mEndTime.Subtract(mStartTime)

        Dim timeScrpt As String = ""
        Dim pointScript As String = ""
        Dim prePoint As String = ""
        Dim i As Integer = mTimes.Count - 1
        Dim span As Integer

        Dim gap As Integer = Math.Floor(mTimes.Count / 100)
        If gap = 0 Then gap = 1
        For i = 0 To mTimes.Count - 1 Step gap
            If Format((mPrices(i) - mMin) / xRange * 100, "0.#") <> prePoint Then
                span = (mTimes(i).Hour - StartTime.Hour) * 3600 + (mTimes(i).Minute - StartTime.Minute) * 60 + mTimes(i).Second - StartTime.Second
                timeScrpt += Format(span / timeRange.TotalSeconds * 100, "0.#") + ","
                pointScript += Format((mPrices(i) - mMin) / xRange * 100, "0.#") + ","
            End If
            prePoint = Format((mPrices(i) - mMin) / xRange * 100, "0.#")
        Next

        '加最後一個點
        i = mTimes.Count - 1
        span = (mTimes(i).Hour - StartTime.Hour) * 3600 + (mTimes(i).Minute - StartTime.Minute) * 60 + mTimes(i).Second - StartTime.Second
        timeScrpt += Format(span / timeRange.TotalSeconds * 100, "0.#") + ","
        pointScript += Format((mPrices(i) - mMin) / xRange * 100, "0.#") + ","

        timeScrpt = timeScrpt.TrimEnd(",")
        script = script + timeScrpt + "|" + pointScript

        'For Each time As DateTime In mTimes
        '    Dim span As Integer = (time.Hour - StartTime.Hour) * 60 + time.Minute - StartTime.Minute
        '    script += Format(span / timeRange.TotalMinutes * 100, "0.##") + ","
        'Next

        'script = script.TrimEnd(",")
        'script += "|"

        'For Each point As Double In mPrices
        '    script += Format((point - mMin) / xRange * 100, "0.##") + ","
        'Next

        script = script.TrimEnd(",")
        script += "&chco="
        script += mColor
        script = script + "&chls=" + Format(mLineWidth, "0.##")

        Return script
    End Function

#End Region

#Region "Axis"

    Private mX As Generic.List(Of String)
    Private mXAxis As New Generic.List(Of String)
    Public Property XAxis() As Generic.List(Of String)
        Get
            Return mXAxis
        End Get
        Set(ByVal value As Generic.List(Of String))
            mXAxis = value
        End Set
    End Property

    Private mYAxis As New Generic.List(Of String)
    Public Property YAxis() As Generic.List(Of String)
        Get
            Return mYAxis
        End Get
        Set(ByVal value As Generic.List(Of String))
            mYAxis = value
        End Set
    End Property

    Private Function GetAxisScription() As String
        '軸刻度標題位置 chxp=
        '<axis_1_index>,<label_1_position>,...,<label_n_position>
        '  |...|
        '<axis_m_index>,<label_1_position>,...,<label_n_position>

        Dim script As String = "&chxt=x,y,r&chxl="
        Dim position As String = "&chxp="

        Dim totalMinutes As Integer = mEndTime.Subtract(mStartTime).TotalMinutes

        '畫時間刻度，若有成交量則不畫在這，畫在成交量圖上
        If mhasVolume Then
            script = "&chxt=y,r&chxl="
        Else
            script += "0:|" + Format(mStartTime, "HH")
            position += "0,3"
            Dim tempTime As Date = mStartTime.AddHours(1) '.AddMinutes(-mStartTime.Minute)

            While tempTime <= mEndTime
                If mWidth <= 400 Then
                    script = script + "|" + Format(tempTime, "HH")
                Else
                    script = script + "|" + Format(tempTime, "HH:mm")
                End If
                position = position + "," + Format(tempTime.Subtract(mStartTime).TotalMinutes / totalMinutes * 100, "0,##")
                tempTime = tempTime.AddHours(1)
            End While
            'script += "|" + Format(mEndTime, "HH:mm")
            'position += ",97"
        End If

        If mIsMaxMinAuto Then

            If mhasVolume Then
                script += "0:"
            Else
                script += "|1:"
            End If

            script += "|" + Format(mMin, "0.##")
            script += "|" + Format(mOpen, "0.##")
            script += "|" + Format(mMax, "0.##")

            If mhasVolume Then
                script += "|1:"
            Else
                script += "|2:"
            End If

            script += "|" + Format((mMin - mOpen) / mOpen, "0.##%")
            script += "|0.00%"
            script += "|" + Format((mMax - mOpen) / mOpen, "0.##%")
        Else

            Dim gap As Double = (mMax - mMin) / 10
            If mhasVolume Then
                script += "0:"
            Else
                script += "|1:"
            End If
            script += "|" + Format(mMin, "0.00")
            script += "|" + Format(mMin + gap, "0.00")
            script += "|" + Format(mMin + gap * 2, "0.00")
            script += "|" + Format(mMin + gap * 3, "0.00")
            script += "|" + Format(mMin + gap * 4, "0.00")
            script += "|" + Format(mOpen, "0.00")
            script += "|" + Format(mMax - gap * 4, "0.00")
            script += "|" + Format(mMax - gap * 3, "0.00")
            script += "|" + Format(mMax - gap * 2, "0.00")
            script += "|" + Format(mMax - gap, "0.00")
            script += "|" + Format(mMax, "0.00")

            If mhasVolume Then
                script += "|1:"
            Else
                script += "|2:"
            End If
            script += "|" + Format((mMin - mOpen) / mOpen, "0.00%")
            script += "|" + Format(-gap * 4 / mOpen, "0.00%")
            script += "|" + Format(-gap * 3 / mOpen, "0.00%")
            script += "|" + Format(-gap * 2 / mOpen, "0.00%")
            script += "|" + Format(-gap / mOpen, "0.00%")
            script += "|0.00%"
            script += "|" + Format(gap / mOpen, "0.00%")
            script += "|" + Format(gap * 2 / mOpen, "0.00%")
            script += "|" + Format(gap * 3 / mOpen, "0.00%")
            script += "|" + Format(gap * 4 / mOpen, "0.00%")
            script += "|" + Format((mMax - mOpen) / mOpen, "0.00%")
        End If

        Return script + position
    End Function


    Private Function GetAxisStyleScription() As String
        'chxs=
        '<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>
        '  |...|
        '<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>

        Dim script As String
        If mhasVolume Then
            script = "&chxs=0,,12|1,,12"
        Else
            script = "&chxs=0,,12|1,,12|2,,12"
        End If
        Return script
    End Function

    Private Function GetGridScription() As String
        '語法 chg= 
        '<x_axis_step_size>,<y_axis_step_size>,<dash_length>,<space_length>,<x_offset>,<y_offset>
        Dim script As String = "&chg="

        Dim xStep As Double = 60 / mEndTime.Subtract(mStartTime).TotalMinutes * 100
        Dim yStep As Double = 10
        If mIsMaxMinAuto Then yStep = 50
        Dim dashLength As Integer = 3
        Dim spaceLength As Integer = 3
        Dim xOffset As Double = 0 'xStep * (60 - mStartTime.Minute) / 60
        'If mStartTime.Minute = 0 Then xOffset = 0
        Dim yOffset As Double = 0

        script = script + Format(xStep, "0.##") + "," + yStep.ToString + "," + _
        dashLength.ToString + "," + _
        spaceLength.ToString + "," + _
        Format(xOffset, "0.##") + "," + _
        yOffset.ToString

        Return script
    End Function
#End Region

    Public Function GetChart() As String
        Dim url As String = "http://chart.apis.google.com/chart?cht=lxy"
        url += GetSize()
        url += GetTitleScript()
        url += GetLineScript()
        url += GetGridScription()
        url += GetAxisScription()
        url += GetAxisStyleScription()
        Return url
    End Function

    Public Function GetVolumeChart() As String
        Dim url As String = "http://chart.apis.google.com/chart?cht=lxy"
        url += GetSize()
        url += GetTitleScript()
        url += GetLineScript()
        url += GetGridScription()
        url += GetAxisScription()
        Return url
    End Function

End Class

''' <summary>
''' 選股條件
''' </summary>
Public Enum SelectionRule
    ThroughInterval
    LargeVolume
    LargeVolumeContinue
    Swallow
    Cover
    MeanCross
    Mean510Cross
    KDCross
    Through5Mean
    Through10Mean
    Through20Mean
    Through50Mean
    Through60Mean
    News
    Combine3
    Combine4
    ThroughHighLow
End Enum

Public Enum Direct
    Rise
    Fall
End Enum