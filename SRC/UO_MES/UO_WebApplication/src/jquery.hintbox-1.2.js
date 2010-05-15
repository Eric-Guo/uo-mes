/**
 * jquery.hintbox.js v1.2
 *
 * @author Tiziano Treccani <treccani.tiziano@tiscali.it>
 * Copyright (c) 2009 Tiziano Treccani - released under MIT License
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:

 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

(function($){
	$.fn.hintbox = function(options){
		// selector
		inputs = this;
	
		// press actions
		var ACTIONS = {
			ENTER: 13,
			LEFT: 37,
			UP: 38,
			RIGHT: 39,
			DOWN: 40,
			SPACE: 32,
			TAB: 9
		}
		
		// default options
		var defaults = {
			backgroundColor: '#3369F9',
			color: '#FFFFFF',
			autoDimentions: true, /* if autoWidth is true,  width option will be equal to css .hint_input class width.  if false, li width will be the specidief one or the default one will be  */
			minChars: 1,
			width: '120px',
			separator: '\n',
			delay: 400, //milliseconds,
			slideDownTime: 0, //milliseconds
			slideUpTime: 0,
			inputClass: 'hintbox_input',
			inputLoadingClass: 'hintbox_loading',
			hintboxContainerClass: 'hintbox_list_container',
			url: '',
			params: { },
			extraParams: '',
			useCache: true,
			matchHint: false,
			sort: false,
			limit: 0,
			onBeforeListLoad: function(){ },
			onListLoaded: function(){ },
			onItemSelected: function(){ }
		}
		// merge options
		var options = jQuery.extend(defaults, options);
		
		// levenshtein distance between words 'a' and 'b'
		var levenshtein = function(a, b){
			var i;
			var j;
			var cost;
			var d = new Array();
		 
			if (a.length == 0){
				return b.length;
			}
		 
			if (b.length == 0){
				return a.length;
			}
		 
			for (i = 0; i <= a.length; i++){
				d[i] = new Array();
				d[i][0] = i;
			}
		 
			for (j = 0; j <= b.length; j++){
				d[0][j] = j;
			}
		 
			for (i = 1; i <= a.length; i++){
				for (j = 1; j <= b.length; j++){

					cost = (a.charAt(i - 1) == b.charAt(j - 1)) ? 0 : 1;
		 
					d[ i ][ j ] = Math.min(d[i - 1][j] + 1, d[i][j - 1] + 1, d[i - 1][j - 1] + cost);
					
					if(i > 1 && j > 1 &&  a.charAt(i - 1) == b.charAt(j - 2) && a.charAt(i - 2) == b.charAt(j - 1)){
						d[i][j] = Math.min(d[i][j], d[i - 2][j - 2] + cost);
					}
				}
			}
		 
			return d[a.length][b.length];
		}
		
		// start service functions
		var bindKeyboard = function(input){
			// manage keyboard behaviours
			jQuery(document).bind('keydown', function(event){
				selectHintElement(event, input);
			});
		}
		
		var unbindKeyboard = function(){
			jQuery(document).unbind('keydown');
		}
		
		var rebindKeyboard = function(input){
			unbindKeyboard();
			bindKeyboard(input);
		}
		
		var getUnsortedList = function(input){
			return getHintListContainer(input).find('ul');
		}
		
		var getHintListContainer = function(input){
			return input.next('.' + options.hintboxContainerClass);
		}
		
		var hintListContainerExists = function(input){
			if (getHintListContainer(input).get() == ''){
				return false;
			}
			return true;
		}
		// end service functions
		
		// init and manage intbox
		var init = function(input){
			var prevVal = '';
			input.attr('autocomplete', 'off');
			input.keyup(function(event){
				if (event.keyCode != ACTIONS.UP && event.keyCode != ACTIONS.DOWN && event.keyCode != ACTIONS.TAB &&
				    event.keyCode != ACTIONS.ENTER && event.keyCode != ACTIONS.LEFT && event.keyCode != ACTIONS.RIGHT){
					
					var currentVal = jQuery.trim(input.val());
					
					if (currentVal != prevVal){
						if(currentVal.length >= options.minChars){
							// load ajax list
							setTimeout(function(){loadList(input);}, options.delay);
						}
						else {
							// hide ajax list
							getHintListContainer(input).slideUp(options.slideUpTime);
						}
					}
					
					// update prevVal
					prevVal = currentVal;
				}
				// hide dropdown list when pressed enter from input text
				if (event.keyCode == ACTIONS.ENTER || event.keyCode == ACTIONS.TAB){
					var container = getHintListContainer(input);
					if (container.get() != ''){
						closeHintResults(input);
					}					
				}
			});
		}
		
		// select li lement by click
		var selectHintElementByClick = function(input){
			if (hintListContainerExists(input)){
				var unsortedList = getUnsortedList(input);
				unsortedList.find('li').click(function(){
					var curLi = jQuery(this);
					input.val(curLi.text());
					closeHintResults(input);
					options.onItemSelected(curLi);
				});
			}
		}
		
		// navigte into hint list and manage li element selection by keyboard
		var selectHintElement = function(event, input){
			if (hintListContainerExists(input)){
				var cssBackup = jQuery.data(input, "cssBackup");
				var unsortedList = getUnsortedList(input);
				var firstLi = unsortedList.find('li:first');
				var lastLi = unsortedList.find('li:last');
				
				// selectd proper li element
				var selectedLi = unsortedList.find('.selected');
				
				// select fist navication li element
				if (selectedLi.get() == ''){
					if (event.keyCode == ACTIONS.DOWN){
						selectedLi = firstLi;
					}
					else if (event.keyCode == ACTIONS.UP){
						selectedLi = lastLi;
					}
					selectLi(selectedLi);
					return;
				}	
				
				// move selection down
				if (event.keyCode == ACTIONS.DOWN){
					deselectLi(selectedLi, cssBackup);
					var nextLi = selectedLi.next('li');
					if (nextLi.get() == ''){
						nextLi = firstLi;	
						input.focus();
						unsortedList.find('.selected').removeClass('selected');
					}
					else {
						selectedLi = nextLi;
						selectLi(selectedLi);
					}
				}
				
				// move selection up
				else if (event.keyCode == ACTIONS.UP){
					deselectLi(selectedLi, cssBackup);
					var prevLi = selectedLi.prev('li');
					if (prevLi.get() == ''){
						prevLi = lastLi;
						input.focus();
						unsortedList.find('.selected').removeClass('selected');
					}
					else {
						selectLi(prevLi);
						selectedLi = prevLi;
					}
				}
				
				// select list value enter
				else if (event.keyCode == ACTIONS.ENTER || event.keyCode == ACTIONS.TAB){
					if (selectedLi.get() != ''){
						input.val(selectedLi.text());
					}
					closeHintResults(input);
					options.onItemSelected(selectedLi);
				}
				
			}
		}
			
		// build url
		var buildUrl = function(input){
			var url = options.url;
			url.indexOf('?') != -1 ? url += '&' : url += '?';
			url += 'q=' + escape(jQuery.trim(input.val()));
			
			// add params
			for (i in options.params){
				url += '&' + i + '=' + escape(jQuery.trim(options.params[i]));
			}
			var extraParams = jQuery.trim(options.extraParams);
			if(extraParams.length > 0){
				if(extraParams.indexOf('&') > 0){
					url += '&' + extraParams;
				}
				else {
					url += extraParams;
				}
			}
			return url;
		}
		
		// create and give the cache, if useCache option is true
		var getCache = function(){
			var cache = jQuery(document).data('hintbox_cache');
			if (cache == undefined){
				var cache = new Array();
				cache.getItem = function(cKey){
					return this[cKey];
				}
				cache.putItem = function(cKey, cValue){
					this[cKey] = cValue;
				}
				cache.hasItem = function(cKey){
					if (this[cKey] == undefined){
						return false;
					}
					return true;
				}
				jQuery(document).data('hintbox_cache', cache);
			}
			return cache;
		}
		
		// jQuery.ajax success handler
		var onAjaxCallSuccess = function(input, queryUrl, html){
			
			input.removeClass(options.inputLoadingClass);
			
			if(jQuery.trim(html).length > 0){
				
				// check cache option
				if(options.useCache){
					var cache = getCache();
					if(!cache.hasItem(queryUrl)){
						cache.putItem(queryUrl, html);
					}
				}
				
				// create list and populate div container and ul pointers
				createList(input, html);
				
				// backup old css li values
				var cssBackup = {
					backgroundColor: getUnsortedList(input).find('li').css('background-color'),
					color: getUnsortedList(input).find('li').css('color')
				}
				jQuery.data(input, 'cssBackup', cssBackup);
				
				// execute necessary operations
				applyOptions(input);
				fixIssues(input);
				highlight(input);
				
				rebindKeyboard(input);
				selectHintElementByClick(input);
				
				// execute onListLoaded()
				options.onListLoaded(getUnsortedList(input));
			}
		}
		
		// load list by ajax call
		var loadList = function(input) {
			input.addClass(options.inputLoadingClass);
			
			// execute onBeforeListLoad()
			options.onBeforeListLoad();
			// create queryUrl
			var queryUrl = buildUrl(input);
			
			var entries = null;
			if(options.useCache){
				var cache = getCache();
				if(cache.hasItem(queryUrl)){
					entries = cache.getItem(queryUrl);
				}
			}
			
			if(entries == null){
				jQuery.ajax({
					url: queryUrl,
					cache: false,
					success: function(html){
						onAjaxCallSuccess(input, queryUrl, html);
					}
				});
			}
			else {
				onAjaxCallSuccess(input, queryUrl, entries);
			}
		}
		
		// positionate ListContainer correctly
		var positionateHintListContainer = function(input){
			var offset = input.offset();
			var container = getHintListContainer(input);
			var unsortedList = getUnsortedList(input);
			
			container.css({
				'top': offset.top + input.outerHeight() - parseInt(input.css('border-bottom-width')),
				'left': offset.left - parseInt(unsortedList.css('border-left-width')) + parseInt(input.css('border-left-width')),
				'position': 'absolute'
			});
		}
		
		// sort entries, if sort option is true
		var sortList = function(input, entries){
			var inputVal = jQuery.trim(input.val());
			var matrix = new Array();
			
			var i = 0;
			jQuery.each(entries, function(){
				var distance = levenshtein(inputVal, jQuery.trim(this));
				if(matrix[i] == undefined){
					matrix[i] = new Array();
				}
				matrix[i] = new Array(distance, jQuery.trim(this));
				i++;
			});
			
			// sort array ascending
			var swap = function(i, j){
				if (parseInt(matrix[j]) < parseInt(matrix[i])){
					var temp = matrix[i];
					matrix[i] = matrix[j]
					matrix[j] = temp;
					if(i > 0){
						swap(i - 1, i);
					}
				}
			}
			for(var j = 0; j < matrix.length -1; j++){
				swap(j, j + 1);
			}
			
			// get only values and not levenshtein distance
			entries = new Array();
			for(var j = 0; j < matrix.length; j++){
				var temp = matrix[j];
				entries.push(temp[1]);
			}
			
			// return sorted entries
			return entries;
		}
		
		// match hint, if matchHint option is true
		var matchHint = function(input, entries){
			var inputVal = jQuery.trim(input.val());
			var matched = new Array();
			
			jQuery.each(entries, function(){
				var curItem = jQuery.trim(this).toLowerCase();
				var toMatch = inputVal.toLowerCase();
				if(curItem.match(toMatch) == toMatch){
					matched.push(jQuery.trim(this));
				}
			});
			
			return matched;
		}
		
		// create div container for ul list
		var createList = function(input, html){
			
			var divContainer = getHintListContainer(input);
			if (divContainer.get() == ''){
				divContainer = jQuery('<div></div>').addClass(options.hintboxContainerClass).css({
					'margin': 0,
					'padding': 0,
					'display': 'none',
					'z-index': 100
				});
			}
			else {
				divContainer.empty();
			}
			
			// create UL dom element
			var ul = jQuery('<ul></ul>').css({'cursor': 'default'});
			var entries = jQuery.trim(html).split(options.separator);
			
			// match hint
			if(options.matchHint){
				entries = matchHint(input, entries);
			}
			
			// sort by levenshtein distance
			if(options.sort){
				entries = sortList(input, entries);
			}
			
			// limit option
			if(options.limit > 0){
				entries = entries.slice(0, options.limit);
			}
			
			jQuery.each(entries, function(){
				ul.append(jQuery('<li></li>').text(jQuery.trim(this)));
			});

			// append ul to div and show
			divContainer.append(ul);
			input.after(divContainer);
			// positionate container
			positionateHintListContainer(input);
			// show container
			divContainer.slideDown(options.slideDownTime);
		}
		
		
		// apply loaded options
		var applyOptions = function(input){
			var unsortedList = getUnsortedList(input);
			if(unsortedList.find('li').length > 0){
				if (options.autoDimentions){
					// ovverride options.width
					var blw = parseInt(input.css('border-left-width'));
					var brw = parseInt(input.css('border-right-width'));
					var w = input.outerWidth();
					options.width = (w - blw - brw) + 'px';
					// normalize li height
					unsortedList.find('li').css({
						'line-height': input.outerHeight() + 'px'
					});	
				}
				unsortedList.css({
					'width': options.width,
					'overflow-x': 'hidden'
				});
			}
			else {
				unsortedList.remove();
			}
		}
		
		
		// close hit results for givel input
		var closeHintResults = function(input){
			unbindKeyboard();
			var container = getHintListContainer(input);
			container.slideUp(options.slideUpTime, function(){
				jQuery(this).remove();
			});
		}
		
		// highlight or not highlight, this is the question :-)
		var highlight = function(input){
			var unsotedList = getUnsortedList(input);
			var cssBackup = jQuery.data(input, 'cssBackup');
			
			unsotedList.find('li').hover(
				function(){
					deselectLi(unsotedList.find('.selected'), cssBackup);
					selectLi(jQuery(this));
				},
				function(){
					deselectLi(jQuery(this), cssBackup);
				}
			);
		}
		
		// hilight current selected li element
		var selectLi = function(li){
			li.css({
				'background-color': options.backgroundColor,
				'color': options.color
			});
			li.addClass('selected');
		}
		
		// deselect pevious selected li element
		var deselectLi = function(li, cssBackup){
			li.css({
				'background-color': cssBackup.backgroundColor,
				'color': cssBackup.color
			});
			li.removeClass('selected');
		}
		// end css behaviours
		
		
		// fix browsers issues
		var fixIssues = function(input){
			// fix i.e. issues
			if (!jQuery.support.boxModel){
				getUnsortedList(input).css({
					'width': (parseInt(options.width) + parseInt(input.css('border-left-width')) * 2) + 'px'
				}).find('li').css({
					'width': (parseInt(options.width) + parseInt(input.css('border-left-width')) * 2) + 'px'
				});
			}
		}
		
		
		// startup jquery.hint for each selected input
		jQuery(inputs).each(function(){
			var input = jQuery(this);
			if (!input.hasClass(options.inputClass)){
				input.addClass(options.inputClass);
			}
			
			// init  main controls and behaviours
			init(input);
			
			input.click(function(){
				rebindKeyboard(input);
			});
			input.focus(function(){
				rebindKeyboard(input);
			});
			input.blur(function(){
				rebindKeyboard(input);
			});
			
			
		});
		
		// returning selector
		return inputs;
		
	};
})(jQuery);